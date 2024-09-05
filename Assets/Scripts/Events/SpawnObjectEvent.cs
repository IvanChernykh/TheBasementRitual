using UnityEngine;

public class SpawnObjectEvent : MonoBehaviour {
    [SerializeField] private GameObject objectToSpawn;
    private bool eventIsTriggered;

    private void OnTriggerEnter(Collider other) {
        if (!eventIsTriggered) {
            if (other.CompareTag("Player")) {
                eventIsTriggered = true;
                objectToSpawn.gameObject.SetActive(true);
            }
        }
    }
}
