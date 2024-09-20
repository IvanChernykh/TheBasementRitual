using System.Collections;
using UnityEngine;

public class DoorEventAction : EventAction {
    [SerializeField] private Door door;
    [Tooltip("Open or close"), SerializeField] private bool openDoor;
    [SerializeField] private bool lockDoor;
    [SerializeField] private string lockMessage;
    [SerializeField] private bool unlockDoor;
    [SerializeField] private float delay;

    public override void ExecuteEvent() {
        if (delay > 0) {
            StartCoroutine(HandleActionWithDelay());
        } else {
            HandleAction();
        }
    }
    private void HandleAction() {
        if (openDoor) {
            door.OpenDoor();
        } else {
            door.CloseDoor();
        }
        if (lockDoor) {
            door.Lock(lockMessage);
        }
        if (unlockDoor) {
            door.Unlock();
        }
    }
    private IEnumerator HandleActionWithDelay() {
        yield return new WaitForSeconds(delay);
        HandleAction();
    }
}
