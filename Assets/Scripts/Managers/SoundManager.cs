using System.Collections;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {

    private class PlayClipSettings {
        public AudioClip Clip { get; set; }
        public Vector3 Position { get; set; }
        public bool Is3D { get; set; } = true;
        public float Volume { get; set; } = 1f;
        public float MinDistance { get; set; } = 1f;
        public float MaxDistance { get; set; } = 5f;
        public float Pitch { get; set; } = 1f;
        public AudioRolloffMode RolloffMode { get; set; } = AudioRolloffMode.Logarithmic;
    };
    private readonly float fadeTolerance = 0.01f;

    private void Awake() {
        InitializeSingleton();
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
        PlayClip(new PlayClipSettings {
            Clip = audioclip,
            Position = position,
            Is3D = true,
            Volume = volume,
            MinDistance = minDistance,
            MaxDistance = maxDistance,
            Pitch = pitch,
            RolloffMode = rolloffMode
        });
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
        PlayClip(new PlayClipSettings {
            Clip = audioClipArr[Random.Range(0, audioClipArr.Length)],
            Position = position,
            Is3D = true,
            Volume = volume,
            MinDistance = minDistance,
            MaxDistance = maxDistance,
            Pitch = pitch,
            RolloffMode = rolloffMode
        });
    }

    public void PlaySound2D(AudioClip audioClip, Vector3 position, float volume = 1f) {
        PlayClip(new PlayClipSettings {
            Clip = audioClip,
            Position = position,
            Volume = volume,
            Is3D = false
        });
    }
    public void PlaySound2D(AudioClip[] audioClipArr, Vector3 position, float volume = 1f) {
        PlayClip(new PlayClipSettings {
            Clip = audioClipArr[Random.Range(0, audioClipArr.Length)],
            Position = position,
            Volume = volume,
            Is3D = false
        });
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
    // fading audio source
    public Coroutine FadeInAudioSource(AudioSource audio, float fadeTime) {
        float initialVolume = audio.volume;
        audio.volume = 0;
        audio.Play();
        return StartCoroutine(FadeAudio(audio, initialVolume, fadeTime));
    }
    public void FadeOutAudioSource(AudioSource audio, float fadeTime, bool pause = false) {
        if (audio.isPlaying) {
            if (pause) {
                StartCoroutine(FadeAudio(audio, 0, fadeTime, pauseAfterFade: true));
            } else {
                StartCoroutine(FadeAudio(audio, 0, fadeTime, stopAfterFade: true));
            }

        }
    }
    public void SwitchAudio(AudioSource audio1, AudioSource audio2, float fadeTime) {
        FadeOutAudioSource(audio1, fadeTime);
        FadeInAudioSource(audio2, fadeTime);
    }
    // volume
    public void IncreaseVolume(AudioSource audioSource, float targetVolume, float fadeTime) {
        StartCoroutine(FadeAudio(audioSource, targetVolume, fadeTime));
    }
    // private methods
    private void PlayClip(PlayClipSettings settings) {
        GameObject newObject = new GameObject("One shot audio");
        newObject.transform.position = settings.Position;
        AudioSource audioSource = (AudioSource)newObject.AddComponent(typeof(AudioSource));

        audioSource.clip = settings.Clip;
        audioSource.spatialBlend = settings.Is3D ? 1f : 0f;
        audioSource.volume = settings.Volume;
        audioSource.pitch = settings.Pitch;
        if (settings.Is3D) {
            audioSource.minDistance = settings.MinDistance;
            audioSource.maxDistance = settings.MaxDistance;
            audioSource.rolloffMode = settings.RolloffMode;
        }
        audioSource.Play();
        Destroy(newObject, settings.Clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    // enumerator
    private IEnumerator FadeAudio(
        AudioSource audioSource,
        float targetVolume,
        float fadeTime,
        bool stopAfterFade = false,
        bool pauseAfterFade = false
        ) {
        float startVolume = audioSource.volume;
        float delta = targetVolume - audioSource.volume;

        while (Mathf.Abs(audioSource.volume - targetVolume) > fadeTolerance) {
            audioSource.volume += delta * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.volume = targetVolume;

        if (stopAfterFade) {
            audioSource.Stop();
            audioSource.volume = startVolume;
        } else if (pauseAfterFade) {
            audioSource.Pause();
            audioSource.volume = startVolume;
        }
    }
}
