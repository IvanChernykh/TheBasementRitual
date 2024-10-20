using UnityEngine;

public class PlayerHasItemCondition : EventCondition {
    [SerializeField] private ItemData requiredItem;
    public override bool IsConditionMet() {
        if (requiredItem == null) {
            return false;
        }
        return PlayerInventory.Instance.HasItem(requiredItem);
    }
}
