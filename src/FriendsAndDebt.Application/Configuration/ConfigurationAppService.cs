using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using FriendsAndDebt.Configuration.Dto;

namespace FriendsAndDebt.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : FriendsAndDebtAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
