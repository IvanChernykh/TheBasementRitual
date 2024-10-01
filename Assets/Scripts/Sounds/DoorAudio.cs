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
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayLocked(Vector3 position, float volume = 0f) {
        PlayDoorSound(locked, position, volume > 0 ? volume : defaultVolume);
    }
    public void PlayClose(Vector3 position) {
        PlayDoorSound(close, position, closeVolume);
    }
    public void PlayOpen(Vector3 position) {
        PlayDoorSound(open, position, defaultVolume);
    }
    public void PlayCloseDoubleDoor(Vector3 position) {
        PlayDoorSound(closeDoubleDoor, position, closeVolume);
    }
    private void PlayDoorSound(AudioClip[] audios, Vector3 position, float volume) {
        SoundManager.Instance.PlaySound(audios, position, volume, minSoundDistance, maxSoundDistance, rolloffMode: AudioRolloffMode.Custom);
    }
}
