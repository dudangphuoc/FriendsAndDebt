using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FriendsAndDebt.Configuration;

namespace FriendsAndDebt.Web.Host.Startup
{
    [DependsOn(
       typeof(FriendsAndDebtWebCoreModule))]
    public class FriendsAndDebtWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public FriendsAndDebtWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FriendsAndDebtWebHostModule).GetAssembly());
        }
    }
}
