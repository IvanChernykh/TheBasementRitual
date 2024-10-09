
using UnityEngine;
public class MainMenuSoundManager : MonoBehaviour {
    [SerializeField] private AudioSource ambient;

    private void Start() {
        SoundManager.Instance.FadeInAudioSource(ambient, fadeTime: 10f);
    }
}
