using UnityEngine;

public class CaseFall : MonoBehaviour {
    [SerializeField] private Transform caseObject;
    [SerializeField] private GameObject invisibleWall;
    // [SerializeField] private AudioSource closeSound;
    private bool eventIsTriggered;

    private void Start() {
        if (!eventIsTriggered) {
            invisibleWall.SetActive(false);
        } else {
            ApplyCaseTransform(caseObject);
            invisibleWall.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (!eventIsTriggered) {
            if (other.CompareTag("Player")) {
                ApplyCaseTransform(caseObject);
                invisibleWall.SetActive(true);
                eventIsTriggered = true;
                // todo: add sound
            }
        }
    }
    private void ApplyCaseTransform(Transform obj) {
        Vector3 fallenPos = new Vector3(12.621f, 0.505f, -21.5f);
        Vector3 fallenRot = new Vector3(-53.578f, 90f, 0f);

        obj.SetPositionAndRotation(fallenPos, Quaternion.Euler(fallenRot));
    }
}
