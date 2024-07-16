using Abp.Application.Services;
using FriendsAndDebt.FriendsAndDebtApp.FriendApp.Dto;
using System.Threading.Tasks;

namespace FriendsAndDebt.FriendsAndDebtApp.FriendApp;

public interface IProjectAppService : IAsyncCrudAppService<FriendModel, long, GetAllFriendModel, CreateFriendModel, UpdateFriendModel, GetFriendModel, DeleteFriendModel>
{
    Task ApproveAsync(long id);
}
