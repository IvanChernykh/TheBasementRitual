using UnityEngine;

public class StopSoundAction : EventAction {
    [Header("AudioSource")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeTime = 0f;

    public override void ExecuteAction() {
        if (fadeTime == 0f) {
            SoundManager.Instance.StopAudioSource(audioSource);
        } else {
            SoundManager.Instance.FadeOutAudioSource(audioSource, fadeTime);
        }
    }
}
