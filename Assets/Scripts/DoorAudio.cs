using UnityEngine;

public class DoorAudio : MonoBehaviour {
    public static DoorAudio Instance = null;
    [SerializeField] private AudioSource lockedAudio;
    [SerializeField] private AudioSource[] closeAudio;
    [SerializeField] private AudioSource[] openAudio;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void PlayLocked() {
        SoundManager.Instance.PlaySource(lockedAudio);
    }
    public void PlayClose() {
        SoundManager.Instance.PlaySource(closeAudio);
    }
    public void PlayOpen() {
        SoundManager.Instance.PlaySource(openAudio);
    }
}
