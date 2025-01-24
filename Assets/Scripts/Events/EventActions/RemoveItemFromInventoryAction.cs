using UnityEngine;

public class RemoveItemFromInvetroryAction : EventAction {
    [SerializeField] private ItemData itemToRemove;
    public override void ExecuteAction() {
        if (PlayerInventory.Instance.HasItem(itemToRemove)) {
            PlayerInventory.Instance.RemoveItem(itemToRemove);
        }
    }
}
