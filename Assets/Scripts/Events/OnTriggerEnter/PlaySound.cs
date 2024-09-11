using UnityEngine;

public class PlaySound : OnTriggerEnterBase {
    [Header("AudioClip")]
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Transform positionToPlay;
    [SerializeField] private float volume = 1f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 10f;


    [Header("AudioSource")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeTime = 0f;

    protected override void HandleEvent() {
        if (audioClip != null) {
            SoundManager.Instance.PlaySound(audioClip, positionToPlay.position, volume, minDistance, maxDistance);
        } else if (audioSource != null) {
            if (fadeTime == 0f) {
                SoundManager.Instance.PlayAudioSource(audioSource);
            } else {
                SoundManager.Instance.FadeInAudioSource(audioSource, fadeTime);
            }
        }
    }
}
