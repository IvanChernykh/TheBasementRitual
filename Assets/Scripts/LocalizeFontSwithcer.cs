using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using TMPro;
using UnityEngine.Localization.Settings;

public class LocalizedFontSwitcher : MonoBehaviour {
    public TextMeshProUGUI targetText;
    public LocalizedAsset<TMP_FontAsset> localizedFont;
    //  public LocalizeStringEvent localizedStringEvent;

    private void OnEnable() {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

        // if (localizedStringEvent != null) {
        // localizedStringEvent.OnUpdateString.AddListener(UpdateFont);
        // }

        UpdateFont();
    }

    private void OnDisable() {
        // Відписуємось від події зміни мови
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;

        // if (localizedStringEvent != null) {
        // localizedStringEvent.OnUpdateString.RemoveListener(UpdateFont);
        // }
    }
    private void OnLocaleChanged(Locale newLocale) {
        Debug.Log("onlocalsechanged");
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