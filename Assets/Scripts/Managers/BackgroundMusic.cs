using UnityEngine;
using Assets.Scripts.Utils;

public class BackgroundMusic : MonoBehaviour {
    public static BackgroundMusic Instance { get; private set; }
    public enum Sounds {
        NoiseAmbient,
        MainAmbient,
        BathroomAmbient,
        ChaseMusic,
        DeepImpacts,
        DeepImpactsStress
    }
    [Header("Ambient")]
    [SerializeField] private AudioSource noiseAmbient;
    [SerializeField] private AudioSource mainAmbient;
    [SerializeField] private AudioSource bathroomAmbient;

    [Header("Music")]
    [SerializeField] private AudioSource chaseMusic;

    [Header("Effects")]
    [SerializeField] private AudioSource deepImpacts;
    [SerializeField] private AudioSource deepImpactsStress;

    // initial volume
    private float noiseAmbientInitialVolume;
    private float mainAmbientInitialVolume;
    private float bathroomAmbientInitialVolume;
    private float chaseMusicInitialVolume;
    private float deepImpactsInitialVolume;
    private float deepImpactsStressInitialVolume;

    // coroutines
    private Coroutine chaseMusicCoroutine;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
    }
    private void Start() {
        noiseAmbientInitialVolume = noiseAmbient.volume;
        mainAmbientInitialVolume = mainAmbient.volume;
        bathroomAmbientInitialVolume = bathroomAmbient.volume;
        chaseMusicInitialVolume = chaseMusic.volume;
        deepImpactsInitialVolume = deepImpacts.volume;
        deepImpactsStressInitialVolume = deepImpactsStress.volume;
    }
    public void Play(Sounds sound, float fadeTime = 0f) {
        AudioSource soundToPlay = GetSoundFromEnum(sound);
        if (fadeTime > 0) {
            SoundManager.Instance.FadeInAudioSource(soundToPlay, fadeTime);
        } else {
            SoundManager.Instance.PlayAudioSource(soundToPlay);
        }
    }
    public void Stop(Sounds sound, float fadeTime = 0f) {
        AudioSource soundToStop = GetSoundFromEnum(sound);
        if (fadeTime > 0) {
            SoundManager.Instance.FadeOutAudioSource(soundToStop, fadeTime);
        } else {
            SoundManager.Instance.StopAudioSource(soundToStop);
        }
    }
    public void Pause(Sounds sound, float fadeTime = 0f) {
        AudioSource soundToPause = GetSoundFromEnum(sound);
        if (fadeTime > 0) {
            SoundManager.Instance.FadeOutAudioSource(soundToPause, fadeTime, pause: true);
        } else {
            SoundManager.Instance.PauseAudioSource(soundToPause);
        }
    }
    public void PlayChaseMusic(float fadeTime = 0f) {
        if (chaseMusic.isPlaying) {
            if (chaseMusicCoroutine != null) {
                StopCoroutine(chaseMusicCoroutine);
                chaseMusicCoroutine = null;
            }
            SoundManager.Instance.IncreaseVolume(chaseMusic, chaseMusicInitialVolume, fadeTime);
            return;
        }
        chaseMusic.volume = chaseMusicInitialVolume;
        if (fadeTime > 0) {
            chaseMusicCoroutine = SoundManager.Instance.FadeInAudioSource(chaseMusic, fadeTime);
        } else {
            SoundManager.Instance.PlayAudioSource(chaseMusic);
        }
    }
    private AudioSource GetSoundFromEnum(Sounds sound) {
        AudioSource receivedSound = sound switch {
            Sounds.NoiseAmbient => noiseAmbient,
            Sounds.MainAmbient => mainAmbient,
            Sounds.BathroomAmbient => bathroomAmbient,
            Sounds.ChaseMusic => chaseMusic,
            Sounds.DeepImpacts => deepImpacts,
            Sounds.DeepImpactsStress => deepImpactsStress,
            _ => noiseAmbient,
        };
        return receivedSound;
    }

    // conditions
    public bool CanPlayDeepImpacts() {
        if (PlayerController.Instance.inChase) {
            return false;
        }
        return true;
    }
}
