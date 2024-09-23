using UnityEngine;

public class CrouchZone : MonoBehaviour {
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerController.Instance.canStandUp = false;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerController.Instance.canStandUp = true;
        }
    }
}
