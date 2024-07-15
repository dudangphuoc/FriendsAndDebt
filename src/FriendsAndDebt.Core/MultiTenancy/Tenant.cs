using Abp.MultiTenancy;
using FriendsAndDebt.Authorization.Users;

namespace FriendsAndDebt.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
