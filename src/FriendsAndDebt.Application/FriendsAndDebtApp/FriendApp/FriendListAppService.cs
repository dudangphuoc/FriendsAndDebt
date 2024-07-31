using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using FriendsAndDebt.Authorization.Users;
using FriendsAndDebt.FAD;
using FriendsAndDebt.FriendsAndDebtApp.FriendApp.Dto;
using FriendsAndDebt.Users.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace FriendsAndDebt.FriendsAndDebtApp.FriendApp;

[AbpAuthorize]
public class FriendListAppService(IRepository<Friend, long> repository, IRepository<User, long> userRepository, IRepository<Board, long> boardRepository) :
    AsyncCrudAppService<Friend, FriendModel, long, GetAllFriendModel, CreateFriendModel, UpdateFriendModel,
        GetFriendModel, DeleteFriendModel>(repository), IProjectAppService
{

    public Task ApproveAsync(long id)
    {
        var entity = Repository.Get(id);
        entity.Approve();
        UnitOfWorkManager.Current.SaveChanges();

        return Task.CompletedTask;

    }

    private Friend SwapValue(Friend model, FriendFilterType type)
    {
        if (type != FriendFilterType.Friends)
        {
            (model.OwnerId, model.UserId, model.Owner, model.User) = (model.UserId, model.OwnerId, model.User, model.Owner);
        }

        return model;
    }

    public override async Task<FriendModel> CreateAsync(CreateFriendModel input)
    {
        var userId = AbpSession.UserId;// tôi

        if (!userId.HasValue)
            throw new UserFriendlyException("User not found");

        //bạn yêu cầu kết bạn với đối tượng nhưng đã tồn tại trước đó => Approve
        //đối tượng yêu cầu với bạn, yêu cầu đã tồn tại rồi  => Approve
        //OwnerId == userId || input.UserId 
        var userRequest = await Repository.GetAll().AsNoTracking()
            .Where(x => (x.UserId == userId && x.OwnerId == input.UserId) || (x.UserId == input.UserId && x.OwnerId == userId))
            .FirstOrDefaultAsync();

        if (userRequest != null)
        {
            await ApproveAsync(userRequest.Id);
            return ObjectMapper.Map<FriendModel>(userRequest);
        }

        // nếu gửi kết bạn mà chưa có yêu cầu nào => send request
        var entity = ObjectMapper.Map<Friend>(input);
        entity.OwnerId = userId ?? 0;
        await Repository.InsertAsync(entity);
        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<FriendModel>(entity);
    }

    public override async Task<PagedResultDto<FriendModel>> GetAllAsync(GetAllFriendModel input)
    {
        CheckGetAllPermission();
        var query = CreateFilteredQuery(input);

        var totalCount = await AsyncQueryableExecuter.CountAsync(query);
        query = ApplySorting(query, input);
        query = ApplyPaging(query, input);

        var entities = await AsyncQueryableExecuter.ToListAsync(query);
        var userId = AbpSession.UserId;

        return new PagedResultDto<FriendModel>(
                totalCount,
                items: ObjectMapper.Map<List<FriendModel>>(input.RequestType != FriendFilterType.SendRequests ? entities.Select(x => SwapValue(x, input.RequestType)) : entities)
        );
    }

    protected override IQueryable<Friend> CreateFilteredQuery(GetAllFriendModel input)
    {
        var userQuery = boardRepository.GetAllIncluding(b => b.Members).Where(b => b.Id == input.BoardId).SelectMany(s => s.Members).Select(s => s.Id);
        var userId = AbpSession.UserId;
        var query = Repository.GetAllIncluding(x => x.User, x => x.Owner).AsNoTracking()
            .WhereIf(input.RequestType == FriendFilterType.Friends, x => ((x.UserId == userId || x.OwnerId == userId) && x.IsApprove == true))
            .WhereIf(input.RequestType == FriendFilterType.FriendRequests, x => (x.UserId == userId) && x.IsApprove == false)
            .WhereIf(input.RequestType == FriendFilterType.SendRequests, x => (x.OwnerId == userId) && x.IsApprove == false)
            .WhereIf(input.BoardId.HasValue && input.RequestType == FriendFilterType.Friends,
                    x => (!userQuery.Contains(x.UserId) || !userQuery.Contains(x.OwnerId))
            )
            .WhereIf(!string.IsNullOrEmpty(input.Keywork), x => x.User.Name.Contains(input.Keywork) || x.Owner.Name.Contains(input.Keywork));
        return query;
    }

    public Task<PagedResultDto<UserDto>> GetUsersAsync(PagedUserResultRequestDto input)
    {


        var query = UserFilteredQuery(input).OrderByDescending(x => x.CreationTime);
        var users = query.PageBy(input).ToList();
        return Task.FromResult(new PagedResultDto<UserDto>(query.Count(), ObjectMapper.Map<List<UserDto>>(users)));
    }

    protected IQueryable<User> UserFilteredQuery(PagedUserResultRequestDto input)
    {
        var userId = AbpSession.GetUserId();
        var frQuery = Repository.GetAll().AsNoTracking()
            .Where(x => x.UserId != userId || x.OwnerId == userId);
        var query = userRepository.GetAll()
            .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                    x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
            .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive).Where(x => !frQuery.Any(a => a.UserId == x.Id || a.OwnerId == x.Id));

        return query;
    }

}