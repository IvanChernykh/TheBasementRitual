using UnityEngine;

public class DoorEventAction : EventAction {
    [SerializeField] private Door door;
    [Tooltip("Open or close"), SerializeField] private bool openDoor;
    public override void ExecuteEvent() {
        if (openDoor) {
            door.OpenDoor();
        } else {
            door.CloseDoor();
        }
    }
}
