using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FriendsAndDebt.Authorization;

namespace FriendsAndDebt
{
    [DependsOn(
        typeof(FriendsAndDebtCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class FriendsAndDebtApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<FriendsAndDebtAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(FriendsAndDebtApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
