using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace FriendsAndDebt.Localization
{
    public static class FriendsAndDebtLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(FriendsAndDebtConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(FriendsAndDebtLocalizationConfigurer).GetAssembly(),
                        "FriendsAndDebt.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
