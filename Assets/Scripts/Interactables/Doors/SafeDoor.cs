using System.Collections;
using UnityEngine;

public class SafeDoor : MonoBehaviour {
    [Header("Door")]
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private bool rotateAroundY;
    private readonly float openSpeed = 200f;
    private readonly float maxOpenAngle = 85f;
    private float currentAngle;
    private bool isOpening;

    public void OpenDoor() {
        StartCoroutine(HandleOpenCoroutine());
    }

    private IEnumerator HandleOpenCoroutine() {
        isOpening = true;

        while (isOpening) {
            float angleToRotate = openSpeed * Time.deltaTime;

            float remainingAngle = maxOpenAngle - currentAngle;
            if (angleToRotate > remainingAngle) {
                angleToRotate = remainingAngle;
                isOpening = false;
            }

            transform.RotateAround(rotationPoint.position, getRotationAxis(), -angleToRotate);
            currentAngle += angleToRotate;

            yield return null;
        }
    }
    private Vector3 getRotationAxis() {
        if (rotateAroundY) {
            return transform.up;
        }
        return transform.forward;
    }
}
