using System.Collections;
using UnityEngine;

public class PlaySoundAction : EventAction {
    [Header("Delay")]
    [SerializeField] private float delay = 0f;
    [SerializeField] private bool randomDelay;
    [SerializeField] private float randomDelayMin;
    [SerializeField] private float randomDelayMax;
    [Header("Reapeat")]
    [SerializeField] private bool repeatSound;
    [SerializeField] private float repeatDelay;
    [SerializeField] private int repeatAmount;
    [Header("AudioClip")]
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Transform positionToPlay;
    [SerializeField] private float volume = 1f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private bool play2D;


    [Header("AudioSource")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeTime = 0f;

    public override void ExecuteEvent() {
        if (delay > 0 || randomDelay) {
            StartCoroutine(PlaySoundWithDelay());
        } else {
            HandlePlaySound();
        }
    }
    private void PlaySound() {
        if (audioClip != null) {
            Vector3 SoundPosition = positionToPlay == null ? transform.position : positionToPlay.position;
            if (play2D) {
                SoundManager.Instance.PlaySound2D(audioClip, SoundPosition, volume);
            } else {
                SoundManager.Instance.PlaySound(audioClip, SoundPosition, volume, minDistance, maxDistance);
            }
        } else if (audioSource != null) {
            if (fadeTime == 0f) {
                SoundManager.Instance.PlayAudioSource(audioSource);
            } else {
                SoundManager.Instance.FadeInAudioSource(audioSource, fadeTime);
            }
        }
    }
    private void HandlePlaySound() {
        if (repeatSound) {
            StartCoroutine(RepeatSound());
        } else {
            PlaySound();
        }
    }
    private IEnumerator PlaySoundWithDelay() {
        yield return new WaitForSeconds(randomDelay ? Random.Range(randomDelayMin, randomDelayMax) : delay);
        HandlePlaySound();
    }
    private IEnumerator RepeatSound() {
        for (int i = 0; i < repeatAmount; i++) {
            PlaySound();
            yield return new WaitForSeconds(repeatDelay);
        }
    }
}
