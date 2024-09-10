using UnityEngine;

public class DoorAudio : MonoBehaviour {
    public static DoorAudio Instance { get; private set; }
    [SerializeField] private AudioClip[] locked;
    [SerializeField] private AudioClip[] close;
    [SerializeField] private AudioClip[] open;
    [SerializeField] private AudioClip[] closeDoubleDoor;
    private float minSoundDistance = .5f;
    private float maxSoundDistance = 5f;
    private float minOpenSoundDistance = .6f;
    private float defaultVolume = 1f;
    private float closeVolume = .5f;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void PlayLocked(Vector3 position) {
        SoundManager.Instance.PlaySound(locked, position, defaultVolume, minSoundDistance, maxSoundDistance);
    }
    public void PlayClose(Vector3 position) {
        SoundManager.Instance.PlaySound(close, position, closeVolume, minSoundDistance, maxSoundDistance);
    }
    public void PlayOpen(Vector3 position) {
        SoundManager.Instance.PlaySound(open, position, defaultVolume, minOpenSoundDistance, maxSoundDistance);
    }
    public void PlayCloseDoubleDoor(Vector3 position) {
        SoundManager.Instance.PlaySound(closeDoubleDoor, position, closeVolume, minSoundDistance, maxSoundDistance);
    }
}
