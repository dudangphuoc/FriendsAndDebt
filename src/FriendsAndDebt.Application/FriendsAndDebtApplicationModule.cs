using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FriendsAndDebt.Authorization;
using FriendsAndDebt.FAD;
using FriendsAndDebt.FriendsAndDebtApp.BoardApp;
using FriendsAndDebt.FriendsAndDebtApp.FriendApp.Dto;

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

            Configuration.Modules.AbpAutoMapper().Configurators.Add((config) =>
            {
                config.CreateMap<Card, CardDto>()
                       .ForMember(x => x.CardOwner, opt => opt.MapFrom(x => x.CardOwner));

                config.CreateMap<Friend, FriendModel>()
                      .ForMember(x => x.OwnerName, opt => opt.MapFrom(x => x.Owner.FullName))
                      .ForMember(x => x.FriendName, opt => opt.MapFrom(x => x.User.FullName));

            });
            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
