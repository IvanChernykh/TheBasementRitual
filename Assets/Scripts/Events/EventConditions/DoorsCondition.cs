using UnityEngine;

public class DoorsCondition : EventCondition {

    [Header("Target Door")]
    [SerializeField] private DoorBase door;

    [Header("Conditions")]
    [SerializeField] private bool isClosed;
    [SerializeField] private bool isOpened;
    [SerializeField] private bool isLocked;

    public override bool IsConditionMet() {
        if (isClosed) {
            return !door.isOpened;
        }
        if (isOpened) {
            return door.isOpened;
        }
        if (isLocked) {
            return !door.isOpened && door.isLocked;
        }
        return true;
    }
}
