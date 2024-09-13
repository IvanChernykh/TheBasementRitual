using System.Collections;
using UnityEngine;

public class DoorEventAction : EventAction {
    [SerializeField] private Door door;
    [Tooltip("Open or close"), SerializeField] private bool openDoor;
    [SerializeField] private float delay;

    public override void ExecuteEvent() {
        if (delay > 0) {
            StartCoroutine(OpenCloseWithDelay());
        } else {
            OpenCloseDoor();
        }
    }
    private void OpenCloseDoor() {
        if (openDoor) {
            door.OpenDoor();
        } else {
            door.CloseDoor();
        }
    }
    private IEnumerator OpenCloseWithDelay() {
        yield return new WaitForSeconds(delay);
        OpenCloseDoor();
    }
}
