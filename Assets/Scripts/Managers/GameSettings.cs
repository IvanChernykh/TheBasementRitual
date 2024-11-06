using UnityEngine;
using Assets.Scripts.Utils;

public class GameSettings : MonoBehaviour {
    public static GameSettings Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    private void Start() {
        if (PlayerPrefs.HasKey(PlayerPrefsConstants.MASTER_VOLUME)) {
            AudioListener.volume = PlayerPrefs.GetFloat(PlayerPrefsConstants.MASTER_VOLUME);
        } else {
            AudioListener.volume = 1;
        }
    }
}
