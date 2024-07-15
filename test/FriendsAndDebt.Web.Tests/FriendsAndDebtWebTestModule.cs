using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FriendsAndDebt.EntityFrameworkCore;
using FriendsAndDebt.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace FriendsAndDebt.Web.Tests
{
    [DependsOn(
        typeof(FriendsAndDebtWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class FriendsAndDebtWebTestModule : AbpModule
    {
        public FriendsAndDebtWebTestModule(FriendsAndDebtEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FriendsAndDebtWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(FriendsAndDebtWebMvcModule).Assembly);
        }
    }
}