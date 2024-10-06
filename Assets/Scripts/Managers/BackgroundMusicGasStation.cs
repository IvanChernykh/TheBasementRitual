using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

public class BackgroundMusicGasStation : MonoBehaviour {
    public static BackgroundMusicGasStation Instance { get; private set; }

    [SerializeField] private AudioSource noiseAmbient;
    [SerializeField] private AudioSource cricketsAmbient;
    [SerializeField] private float fadeInTime = 12f;
    private readonly float delay = 25f;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
