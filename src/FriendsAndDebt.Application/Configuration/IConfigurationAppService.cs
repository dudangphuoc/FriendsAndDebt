using System.Threading.Tasks;
using FriendsAndDebt.Configuration.Dto;

namespace FriendsAndDebt.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
