using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using FriendsAndDebt.FAD;
using FriendsAndDebt.FriendsAndDebtApp.FriendApp.Dto;
using System.Threading.Tasks;

namespace FriendsAndDebt.FriendsAndDebtApp.FriendApp;

[AbpAuthorize]
public class FriendListAppService(IRepository<Friend, long> repository) : AsyncCrudAppService<Friend, FriendModel, long, GetAllFriendModel, CreateFriendModel, UpdateFriendModel, GetFriendModel, DeleteFriendModel>(repository), IProjectAppService
{
    public Task ApproveAsync(long id)
    {
        var entity = Repository.Get(id);
        entity.Approve();
        UnitOfWorkManager.Current.SaveChanges();

        return Task.CompletedTask;
    }
}