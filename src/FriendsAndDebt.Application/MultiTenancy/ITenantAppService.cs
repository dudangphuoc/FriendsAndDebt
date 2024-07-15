using Abp.Application.Services;
using FriendsAndDebt.MultiTenancy.Dto;

namespace FriendsAndDebt.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

