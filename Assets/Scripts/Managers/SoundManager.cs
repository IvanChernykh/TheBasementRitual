using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    public void PlaySound(AudioClip[] audioClipArr, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArr[Random.Range(0, audioClipArr.Length)], position, volume);
    }
    public void PlaySound(AudioClip audioclip, Vector3 position, float volume = 1, float minDistance = 1, float maxDistance = 5f) {
        PlayClip3D(audioclip, position, volume, minDistance, maxDistance);
    }
    public void PlaySound(AudioClip[] audioClipArr, Vector3 position, float volume = 1, float minDistance = 1, float maxDistance = 5f) {
        PlayClip3D(audioClipArr[Random.Range(0, audioClipArr.Length)], position, volume, minDistance, maxDistance);
    }

    public void PlaySound2D(AudioClip audioClip, Vector3 position, float volume = 1f) {
        PlayClip2D(audioClip, position, volume);
    }
    public void PlaySound2D(AudioClip[] audioClipArr, Vector3 position, float volume = 1f) {
        PlayClip2D(audioClipArr[Random.Range(0, audioClipArr.Length)], position, volume);
    }
    public void PlayAudioSource(AudioSource audioSource) {
        audioSource.Play();
    }
    public void PlayAudioSource(AudioSource[] audioSources) {
        audioSources[Random.Range(0, audioSources.Length)].Play();
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
    private void PlayClip3D(AudioClip clip, Vector3 position, float volume = 1, float minDistance = 1, float maxDistance = 5f) {
        GameObject newObject = new GameObject("One shot audio");
        newObject.transform.position = position;
        AudioSource audioSource = (AudioSource)newObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.Play();
        Destroy(newObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }
}
