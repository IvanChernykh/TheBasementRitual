using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using Assets.Scripts.Utils;

public class LanguageSelector : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI languageText;
    private List<Locale> availableLocales;
    private int currentLocaleIndex;

    private void Start() {
        availableLocales = LocalizationSettings.AvailableLocales.Locales;

        currentLocaleIndex = PlayerPrefs.GetInt(PlayerPrefsConstants.LANGUAGE, 0);

        if (currentLocaleIndex < 0 || currentLocaleIndex >= availableLocales.Count) {
            currentLocaleIndex = 0;
        }

        SetLanguage(currentLocaleIndex);
    }

    public void SwitchLanguageLeft() {
        currentLocaleIndex = (currentLocaleIndex - 1 + availableLocales.Count) % availableLocales.Count;
        SetLanguage(currentLocaleIndex);
    }

    public void SwitchLanguageRight() {
        currentLocaleIndex = (currentLocaleIndex + 1) % availableLocales.Count;
        SetLanguage(currentLocaleIndex);
    }

    private void SetLanguage(int index) {
        LocalizationSettings.SelectedLocale = availableLocales[index];
        PlayerPrefs.SetInt(PlayerPrefsConstants.LANGUAGE, index);
        PlayerPrefs.Save();
        UpdateLanguageText();
    }

    private void UpdateLanguageText() {
        languageText.text = LocalizationSettings.SelectedLocale.Identifier.CultureInfo.NativeName;
    }
}