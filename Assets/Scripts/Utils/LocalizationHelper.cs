using UnityEngine.Localization;

namespace Assets.Scripts.Utils {
    public static class LocalizationHelper {
        public static string GetLocalizedString(string key, string table) {
            return new LocalizedString { TableReference = table, TableEntryReference = key }.GetLocalizedString();
        }

        public static string ConcatLocalizedString(string key1, string key2, string table) {
            string localizedString1 = GetLocalizedString(key1, table);
            string localizedString2 = GetLocalizedString(key2, table);
            return $"{localizedString1} {localizedString2}";
        }

        public static string LocalizeTooltip(string key) {
            return GetLocalizedString(key, LocalizationTables.Tooltips);
        }
        public static string LocalizeTooltip(string key1, string key2) {
            return ConcatLocalizedString(key1, key2, LocalizationTables.Tooltips);
        }

        // public static string GenerateLocalizedString(string templateKey, string table, params object[] args) {
        //     string template = GetLocalizedString(templateKey, table);
        //     return string.Format(template, args);
        // }
    }
}
