using UnityEngine;

public class PlaySoundAction : EventAction {
    [Header("AudioClip")]
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Transform positionToPlay;
    [SerializeField] private float volume = 1f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 10f;


    [Header("AudioSource")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeTime = 0f;

    public override void ExecuteEvent() {
        if (audioClip != null) {
            Vector3 SoundPosition = positionToPlay == null ? transform.position : positionToPlay.position;
            SoundManager.Instance.PlaySound(audioClip, SoundPosition, volume, minDistance, maxDistance);
        } else if (audioSource != null) {
            if (fadeTime == 0f) {
                SoundManager.Instance.PlayAudioSource(audioSource);
            } else {
                SoundManager.Instance.FadeInAudioSource(audioSource, fadeTime);
            }
        }
    }
}
