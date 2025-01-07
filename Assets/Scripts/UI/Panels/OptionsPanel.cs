using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TabsEnum {
    Controls,
    Sound,
    Language
}

public class OptionsPanel : MonoBehaviour {
    public static OptionsPanel Instance { get; private set; }

    [SerializeField] private GameObject container;

    [Header("Tabs")]
    [SerializeField] private GameObject controlsTab;
    [SerializeField] private GameObject soundTab;
    [SerializeField] private GameObject languageTab;


    [Header("Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider mouseSensitivitySlider;


    [Header("Buttons UI")]
    [SerializeField] private TextMeshProUGUI controlTabBtnText;
    [SerializeField] private TextMeshProUGUI soundTabBtnText;
    [SerializeField] private TextMeshProUGUI languageTabBtnText;
    [SerializeField] private TextMeshProUGUI backBtnText;
    [SerializeField] private TextMeshProUGUI arrowLeftBtnText;
    [SerializeField] private TextMeshProUGUI arrowRightBtnText;

    public bool IsActive { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start() {
        if (PlayerPrefs.HasKey(PlayerPrefsConstants.MASTER_VOLUME)) {
            masterVolumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsConstants.MASTER_VOLUME);
        }
        if (PlayerPrefs.HasKey(PlayerPrefsConstants.MOUSE_SENSITIVITY)) {
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat(PlayerPrefsConstants.MOUSE_SENSITIVITY);
        }
        masterVolumeSlider.onValueChanged.AddListener((val) => {
            OnChangeMasterVolume(val);
        });
        mouseSensitivitySlider.onValueChanged.AddListener((val) => {
            OnChangeMouseSensitivity(val);
        });
    }

    public void Show() {
        container.SetActive(true);
        IsActive = true;
    }
    public void Hide() {
        controlTabBtnText.color = Color.white;
        soundTabBtnText.color = Color.white;
        languageTabBtnText.color = Color.white;

        arrowLeftBtnText.color = Color.white;
        arrowRightBtnText.color = Color.white;
        backBtnText.color = Color.white;
        container.SetActive(false);
        IsActive = false;
    }

    public void OpenControlTab() {
        if (controlsTab.activeInHierarchy) {
            return;
        }
        controlsTab.SetActive(true);

        soundTab.SetActive(false);
        languageTab.SetActive(false);
    }
    public void OpenSoundTab() {
        if (soundTab.activeInHierarchy) {
            return;
        }
        soundTab.SetActive(true);

        controlsTab.SetActive(false);
        languageTab.SetActive(false);
    }
    public void OpenLanguageTab() {
        if (languageTab.activeInHierarchy) {
            return;
        }
        languageTab.SetActive(true);

        controlsTab.SetActive(false);
        soundTab.SetActive(false);
    }

    private void OnChangeMasterVolume(float val) {
        AudioListener.volume = val;
        PlayerPrefs.SetFloat(PlayerPrefsConstants.MASTER_VOLUME, val);
        PlayerPrefs.Save();
    }
    private void OnChangeMouseSensitivity(float val) {
        PlayerPrefs.SetFloat(PlayerPrefsConstants.MOUSE_SENSITIVITY, val);
        PlayerPrefs.Save();
    }
}
