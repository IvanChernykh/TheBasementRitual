using UnityEngine;

public class PlayerDoesNotHaveAnItemCondition : EventCondition {
    [SerializeField] private ItemData requiredItem;
    public override bool IsConditionMet() {
        if (requiredItem == null) {
            return true;
        }
        return !PlayerInventory.Instance.items.Contains(requiredItem);
    }
}
