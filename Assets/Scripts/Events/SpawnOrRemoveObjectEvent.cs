using UnityEngine;

public class SpawnObjectEvent : MonoBehaviour {
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private GameObject objectToRemove;
    private bool eventIsTriggered;

    private void OnTriggerEnter(Collider other) {
        if (!eventIsTriggered) {
            if (other.CompareTag("Player")) {
                eventIsTriggered = true;
                if (objectToSpawn != null) {
                    objectToSpawn.SetActive(true);
                }
                if (objectToRemove != null) {
                    objectToRemove.SetActive(false);
                }
            }
        }
    }
}
