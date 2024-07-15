using System.Threading.Tasks;
using Abp.Application.Services;
using FriendsAndDebt.Sessions.Dto;

namespace FriendsAndDebt.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
