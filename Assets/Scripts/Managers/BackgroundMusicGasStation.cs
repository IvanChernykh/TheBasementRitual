using System.Collections;
using UnityEngine;

public class BackgroundMusicGasStation : Singleton<BackgroundMusicGasStation> {

    [SerializeField] private AudioSource noiseAmbient;
    [SerializeField] private AudioSource cricketsAmbient;
    [SerializeField] private float fadeInTime = 12f;
    private readonly float delay = 25f;

    private void Awake() {
        InitializeSingleton();
    }
    private void Start() {
        StartCoroutine(PlayWithDelay());
    }
    private IEnumerator PlayWithDelay() {
        yield return new WaitForSeconds(delay);

        SoundManager.Instance.FadeInAudioSource(noiseAmbient, fadeInTime);
        SoundManager.Instance.FadeInAudioSource(cricketsAmbient, fadeInTime);
    }
}
