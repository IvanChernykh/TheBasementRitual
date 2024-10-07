using UnityEngine;

public class WakeUpZone : MonoBehaviour {
    [SerializeField] private UnconScreen unconScreen;
    private bool isTriggered;
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            if (!isTriggered) {
                unconScreen.gameObject.SetActive(true);
                unconScreen.WakeUp();
                isTriggered = true;
            } else {
                Destroy(gameObject);
            }
        }
    }
}
