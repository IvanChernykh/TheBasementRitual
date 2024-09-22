using UnityEngine;

public class DeepImpactsZone : MonoBehaviour {
    [SerializeField] private AudioSource deepImpacts;
    [SerializeField] private AudioSource deepImpactsStress;
    [SerializeField] private bool stressZone;
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float fadeOutTime = 1f;
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            HandlePlay();
        }
    }
    private void OnTriggerExit() {
        Stop();
    }
    private void HandlePlay() {
        if (stressZone) {
            if (!deepImpactsStress.isPlaying) {
                BackgroundMusic.Instance.Play(BackgroundMusic.Sounds.DeepImpactsStress, fadeInTime);
            }
        } else {
            if (!deepImpacts.isPlaying) {
                BackgroundMusic.Instance.Play(BackgroundMusic.Sounds.DeepImpacts, fadeInTime);
            }
        }
    }
    private void Stop() {
        if (stressZone) {
            BackgroundMusic.Instance.Stop(BackgroundMusic.Sounds.DeepImpactsStress, fadeOutTime);
        } else {
            BackgroundMusic.Instance.Stop(BackgroundMusic.Sounds.DeepImpacts, fadeOutTime);
        }
    }
}
