using System.Collections;
using UnityEngine;
using Assets.Scripts.Utils;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
    }
    // AudioClip
    public void PlaySound(
        AudioClip audioclip,
        Vector3 position,
        float volume = 1,
        float minDistance = 1,
        float maxDistance = 5f,
        float pitch = 1f,
        AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic
        ) {
        PlayClip3D(audioclip, position, volume, minDistance, maxDistance, pitch, rolloffMode);
    }
    public void PlaySound(
        AudioClip[] audioClipArr,
        Vector3 position,
        float volume = 1,
        float minDistance = 1,
        float maxDistance = 5f,
        float pitch = 1f,
        AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic
        ) {
        PlaySound(audioClipArr[Random.Range(0, audioClipArr.Length)], position, volume, minDistance, maxDistance, pitch, rolloffMode);
    }

    public void PlaySound2D(AudioClip audioClip, Vector3 position, float volume = 1f) {
        PlayClip2D(audioClip, position, volume);
    }
    public void PlaySound2D(AudioClip[] audioClipArr, Vector3 position, float volume = 1f) {
        PlayClip2D(audioClipArr[Random.Range(0, audioClipArr.Length)], position, volume);
    }
    // AudioSource
    public void PlayAudioSource(AudioSource audioSource) {
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }
    public void PlayAudioSource(AudioSource[] audioSources) {
        PlayAudioSource(audioSources[Random.Range(0, audioSources.Length)]);
    }
    public void StopAudioSource(AudioSource audio) {
        if (audio.isPlaying) {
            audio.Stop();
        }
    }
    public void PauseAudioSource(AudioSource audio) {
        if (audio.isPlaying) {
            audio.Pause();
        }
    }
    public void FadeOutAudioSource(AudioSource audio, float fadeTime) {
        if (audio.isPlaying) {
            StartCoroutine(FadeOut(audio, fadeTime));
        }
    }
    public void FadeOutPauseAudioSource(AudioSource audio, float fadeTime) {
        if (audio.isPlaying) {
            StartCoroutine(FadeOutPause(audio, fadeTime));
        }
    }
    public void FadeInAudioSource(AudioSource audio, float fadeTime) {
        float initialVolume = audio.volume;
        audio.volume = 0;
        audio.Play();
        StartCoroutine(FadeIn(audio, initialVolume, fadeTime));
    }
    public void SwitchAudio(AudioSource audio1, AudioSource audio2, float fadeTime) {
        FadeOutAudioSource(audio1, fadeTime);
        FadeInAudioSource(audio2, fadeTime);
    }
    public void IncreaseVolume(AudioSource audioSource, float targetVolume, float fadeTime) {
        StartCoroutine(FadeIn(audioSource, targetVolume, fadeTime));
    }
    // private methods
    private void PlayClip2D(AudioClip clip, Vector3 position, float volume = 1) {
        GameObject newObject = new GameObject("One shot audio");
        newObject.transform.position = position;
        AudioSource audioSource = (AudioSource)newObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 0f;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(newObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }
    private void PlayClip3D(
        AudioClip clip,
        Vector3 position,
        float volume = 1,
        float minDistance = 1,
        float maxDistance = 5f,
        float pitch = 1f,
        AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic
        ) {
        GameObject newObject = new GameObject("One shot audio");
        newObject.transform.position = position;
        AudioSource audioSource = (AudioSource)newObject.AddComponent(typeof(AudioSource));

        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = rolloffMode;

        audioSource.Play();
        Destroy(newObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    // enumerators
    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
    private IEnumerator FadeOutPause(AudioSource audioSource, float fadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Pause();
        audioSource.volume = startVolume;
    }
    private IEnumerator FadeIn(AudioSource audioSource, float targetVolume, float fadeTime) {
        while (audioSource.volume < targetVolume) {
            audioSource.volume += Time.deltaTime / fadeTime * targetVolume;
            yield return null;
        }
        audioSource.volume = targetVolume;
    }
}
