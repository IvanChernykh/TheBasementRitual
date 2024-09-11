using UnityEngine;

public class ClosingDoorEvent : MonoBehaviour {
    private bool eventIsTriggered;
    [SerializeField] private Transform door;
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private GameObject roomLight;
    [SerializeField] private float openSpeed = 140f;
    [SerializeField] private AudioClip closeSound;

    private bool isClosed;
    private float maxOpenAngle = 30f;
    private float currentAngle;


    private void Start() {
        door.RotateAround(rotationPoint.position, door.up, maxOpenAngle);
        currentAngle = maxOpenAngle;
    }
    private void Update() {
        if (eventIsTriggered && !isClosed) {
            HandleClose();
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (!eventIsTriggered) {
            if (other.CompareTag("Player")) {
                eventIsTriggered = true;
                SoundManager.Instance.PlaySound(closeSound, door.position);
            }
        }
    }

    private void HandleClose() {
        float angleToRotate = openSpeed * Time.deltaTime;
        float remainingAngle = currentAngle;
        if (angleToRotate >= remainingAngle) {
            angleToRotate = remainingAngle;
            isClosed = true;
            roomLight.SetActive(false);
        }
        door.RotateAround(rotationPoint.position, door.up, -angleToRotate);
        currentAngle -= angleToRotate;
    }
}
