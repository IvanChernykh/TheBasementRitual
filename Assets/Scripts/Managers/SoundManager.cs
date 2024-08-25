using System.Collections;
using System.Collections.Generic;
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
}
