using UnityEngine;
using UnityEngine.Localization;
using TMPro;
using UnityEngine.Localization.Settings;

public class LocalizedFontSwitcher : MonoBehaviour {
    public TextMeshProUGUI targetText;
    public LocalizedAsset<TMP_FontAsset> localizedFont;

    private void OnEnable() {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        UpdateFont();
    }

    private void OnDisable() {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }
    private void OnLocaleChanged(Locale newLocale) {
        UpdateFont();
    }

    private void UpdateFont() {
        localizedFont.LoadAssetAsync().Completed += handle => {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded) {
                targetText.font = handle.Result;
            } else {
                Debug.LogError("Failed to load font for locale: " + LocalizationSettings.SelectedLocale);
            }
        };
    }
}
