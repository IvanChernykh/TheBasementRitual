using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour {
    public static OptionsPanel Instance { get; private set; }

    [SerializeField] private GameObject container;

    [SerializeField] private Slider masterVolumeSlider;

    [SerializeField] private TextMeshProUGUI backBtnText;

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
        masterVolumeSlider.onValueChanged.AddListener((val) => {
            OnChangeMasterVolume(val);
        });
    }

    public void Show() {
        container.SetActive(true);
        IsActive = true;
    }
    public void Hide() {
        backBtnText.color = Color.white;
        container.SetActive(false);
        IsActive = false;
    }

    private void OnChangeMasterVolume(float val) {
        AudioListener.volume = val;
        PlayerPrefs.SetFloat(PlayerPrefsConstants.MASTER_VOLUME, val);
    }
}
