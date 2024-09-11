using UnityEngine;

public class StartAmbient : MonoBehaviour {
    [SerializeField] private AudioSource ambient;
    private bool eventIsTriggered;

    private void OnTriggerEnter(Collider other) {
        if (!eventIsTriggered) {
            if (other.CompareTag("Player")) {
                eventIsTriggered = true;
                SoundManager.Instance.FadeInAudioSource(ambient, 10f);
            }
        }
    }
}
