using System.Threading.Tasks;
using Abp.Application.Services;
using FriendsAndDebt.Authorization.Accounts.Dto;

namespace FriendsAndDebt.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
