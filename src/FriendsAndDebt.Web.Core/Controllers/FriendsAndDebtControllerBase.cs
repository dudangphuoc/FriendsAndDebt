using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace FriendsAndDebt.Controllers
{
    public abstract class FriendsAndDebtControllerBase: AbpController
    {
        protected FriendsAndDebtControllerBase()
        {
            LocalizationSourceName = FriendsAndDebtConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
