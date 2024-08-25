using UnityEngine;

public class Door : Interactable {
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private float openSpeed = 120f;
    private bool isOpened;
    private bool openingDoor;
    private float maxOpenAngle = 90f;
    private float currentAngle = 0f;

    private void Update() {
        HandleOpen();
    }

    public override void Interact() {
        openingDoor = !openingDoor;
    }
    private void HandleOpen() {
        if (openingDoor == isOpened) {
            return;
        }
        if (openingDoor && currentAngle < maxOpenAngle) {
            float angleToRotate = openSpeed * Time.deltaTime;
            float remainingAngle = maxOpenAngle - currentAngle;
            if (angleToRotate > remainingAngle) {
                angleToRotate = remainingAngle;
                isOpened = true;
            }
            transform.RotateAround(rotationPoint.position, transform.up, angleToRotate);
            currentAngle += angleToRotate;
        } else {
            float angleToRotate = openSpeed * Time.deltaTime;
            float remainingAngle = currentAngle;
            if (angleToRotate > remainingAngle) {
                angleToRotate = remainingAngle;
                isOpened = false;
            }
            transform.RotateAround(rotationPoint.position, transform.up, -angleToRotate);
            currentAngle -= angleToRotate;
        }
    }
}
