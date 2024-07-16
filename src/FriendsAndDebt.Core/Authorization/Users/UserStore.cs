using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using FriendsAndDebt.Authorization.Roles;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace FriendsAndDebt.Authorization.Users
{
    public class UserStore : AbpUserStore<Role, User>
    {
        IUnitOfWorkManager _unitOfWorkManager;
        public UserStore(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserClaim, long> userClaimRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
            IRepository<UserToken, long> userTokenRepository
        )
            : base(unitOfWorkManager,
                  userRepository,
                  roleRepository,
                  userRoleRepository,
                  userLoginRepository,
                  userClaimRepository,
                  userPermissionSettingRepository,
                  userOrganizationUnitRepository,
                  organizationUnitRoleRepository,
                  userTokenRepository
            )
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<User> GetUserFromDatabaseAsync(string userName)
        {
            using (var uow = _unitOfWorkManager.Begin(new UnitOfWorkOptions
            {
                Scope = TransactionScopeOption.Suppress
            }))
            {
                var entity = UserRepository.GetAll().Where(x => x.NormalizedUserName == userName.ToUpperInvariant()).FirstOrDefault();
                if (entity == null)
                    throw new EntityNotFoundException(typeof(User), userName);

                await uow.CompleteAsync();
                return entity;
            }
        }



    }
}
