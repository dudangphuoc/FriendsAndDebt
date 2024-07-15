using Abp.Authorization;
using FriendsAndDebt.Authorization.Roles;
using FriendsAndDebt.Authorization.Users;

namespace FriendsAndDebt.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
