using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class LanguageSelector : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI languageText;
    private List<Locale> availableLocales;
    private int currentLocaleIndex;

    private void Start() {
        availableLocales = LocalizationSettings.AvailableLocales.Locales;
        currentLocaleIndex = availableLocales.IndexOf(LocalizationSettings.SelectedLocale);
        UpdateLanguageText();
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
        UpdateLanguageText();
    }

    private void UpdateLanguageText() {
        languageText.text = LocalizationSettings.SelectedLocale.Identifier.CultureInfo.NativeName;
    }
}