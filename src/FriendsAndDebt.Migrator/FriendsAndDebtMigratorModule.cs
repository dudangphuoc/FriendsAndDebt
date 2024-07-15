using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FriendsAndDebt.Configuration;
using FriendsAndDebt.EntityFrameworkCore;
using FriendsAndDebt.Migrator.DependencyInjection;

namespace FriendsAndDebt.Migrator
{
    [DependsOn(typeof(FriendsAndDebtEntityFrameworkModule))]
    public class FriendsAndDebtMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public FriendsAndDebtMigratorModule(FriendsAndDebtEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(FriendsAndDebtMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                FriendsAndDebtConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FriendsAndDebtMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
