using FriendsAndDebt.Debugging;

namespace FriendsAndDebt
{
    public class FriendsAndDebtConsts
    {
        public const string LocalizationSourceName = "FriendsAndDebt";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "9f4384a925454b4bafa1d47dd0fb5122";
    }
}
