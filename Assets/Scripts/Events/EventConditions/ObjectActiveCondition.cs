using UnityEngine;

public class ObjectActiveCondition : EventCondition {
    [SerializeField] private GameObject targetObject;
    [SerializeField] private bool isActive = true;
    [SerializeField] private bool isNotActive = false;
    public override bool IsConditionMet() {
        if (isActive) {
            return targetObject.activeInHierarchy;
        }
        if (isNotActive) {
            return !targetObject.activeInHierarchy;
        }
        return true;
    }
}
