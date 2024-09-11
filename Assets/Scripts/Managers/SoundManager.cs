using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }
    // AudioClip
    public void PlaySound(
        AudioClip audioclip,
        Vector3 position,
        float volume = 1,
        float minDistance = 1,
        float maxDistance = 5f,
        float pitch = 1f
        ) {
        PlayClip3D(audioclip, position, volume, minDistance, maxDistance, pitch);
    }
    public void PlaySound(
        AudioClip[] audioClipArr,
        Vector3 position,
        float volume = 1,
        float minDistance = 1,
        float maxDistance = 5f,
        float pitch = 1f
        ) {
        PlaySound(audioClipArr[Random.Range(0, audioClipArr.Length)], position, volume, minDistance, maxDistance, pitch);
    }

    public void PlaySound2D(AudioClip audioClip, Vector3 position, float volume = 1f) {
        PlayClip2D(audioClip, position, volume);
    }
    public void PlaySound2D(AudioClip[] audioClipArr, Vector3 position, float volume = 1f) {
        PlayClip2D(audioClipArr[Random.Range(0, audioClipArr.Length)], position, volume);
    }
    // AudioSource
    public void PlayAudioSource(AudioSource audioSource) {
        audioSource.Play();
    }
    public void PlayAudioSource(AudioSource[] audioSources) {
        audioSources[Random.Range(0, audioSources.Length)].Play();
    }
    public void StopAudioSource(AudioSource audioSource) {
        audioSource.Stop();
    }
    public void FadeOutAudioSource(AudioSource audio, float fadeTime) {
        if (audio.isPlaying) {
            StartCoroutine(FadeOut(audio, fadeTime));
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
    private void PlayClip3D(AudioClip clip, Vector3 position, float volume = 1, float minDistance = 1, float maxDistance = 5f, float pitch = 1f) {
        GameObject newObject = new GameObject("One shot audio");
        newObject.transform.position = position;
        AudioSource audioSource = (AudioSource)newObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.Play();
        Destroy(newObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }


    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();
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
