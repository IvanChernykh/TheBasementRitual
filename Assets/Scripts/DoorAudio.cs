using UnityEngine;
using Assets.Scripts.Utils;

public class DoorAudio : MonoBehaviour {
    public static DoorAudio Instance { get; private set; }
    [Header("Sounds")]
    [SerializeField] private AudioClip[] locked;
    [SerializeField] private AudioClip[] close;
    [SerializeField] private AudioClip[] open;
    [SerializeField] private AudioClip[] closeDoubleDoor;
    [Header("Options")]
    [SerializeField] private float minSoundDistance = .8f;
    [SerializeField] private float maxSoundDistance = 6f;
    [SerializeField] private float closeVolume = .5f;
    private float defaultVolume = 1f;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
    }

    public void PlayLocked(Vector3 position) {
        SoundManager.Instance.PlaySound(locked, position, defaultVolume, minSoundDistance, maxSoundDistance);
    }
    public void PlayClose(Vector3 position) {
        SoundManager.Instance.PlaySound(close, position, closeVolume, minSoundDistance, maxSoundDistance);
    }
    public void PlayOpen(Vector3 position) {
        SoundManager.Instance.PlaySound(open, position, defaultVolume, minSoundDistance, maxSoundDistance);
    }
    public void PlayCloseDoubleDoor(Vector3 position) {
        SoundManager.Instance.PlaySound(closeDoubleDoor, position, closeVolume, minSoundDistance, maxSoundDistance);
    }
}
