using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using FriendsAndDebt.Authorization.Users;
using FriendsAndDebt.FAD;
using FriendsAndDebt.FriendsAndDebtApp.FriendApp.Dto;
using FriendsAndDebt.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendsAndDebt.FriendsAndDebtApp.FriendApp;

[AbpAuthorize]
public class FriendListAppService(IRepository<Friend, long> repository, IRepository<User, long> userRepository) :
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

    private Friend SwapValue(Friend model, long userId)
    {
        if (model.OwnerId != userId)
            (model.OwnerId, model.UserId, model.Owner, model.User) = (model.UserId, model.OwnerId, model.User, model.Owner);
        return model;
    }

    public override async Task<FriendModel> CreateAsync(CreateFriendModel input)
    {
        var userId = AbpSession.UserId;
        if (!userId.HasValue)
            throw new UserFriendlyException("User not found");

        //nếu gửi kết bạn với người đã gửi kết bạn trước đó => Approve 
        var userRequest = Repository.GetAll().FirstOrDefault(x => x.UserId == userId);
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
                items: ObjectMapper.Map<List<FriendModel>>(input.RequestType == FriendFilterType.Friends ? entities.Select(x => SwapValue(x, userId ?? 0)) : entities)
        );
    }

    protected override IQueryable<Friend> CreateFilteredQuery(GetAllFriendModel input)
    {
        var userId = AbpSession.UserId;
        var query = Repository.GetAllIncluding(x => x.User, x => x.Owner)
            .WhereIf(input.RequestType == FriendFilterType.Friends, x => ((x.UserId == userId || x.OwnerId == userId) && x.IsApprove == true))
            .WhereIf(input.RequestType == FriendFilterType.FriendRequests, x => (x.UserId == userId) && x.IsApprove == false)
            .WhereIf(input.RequestType == FriendFilterType.SendRequests, x => (x.OwnerId == userId) && x.IsApprove == false);

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
        var query = userRepository.GetAll()
            .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                    x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
            .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);

        return query;
    }

}