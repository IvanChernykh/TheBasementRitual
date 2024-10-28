using UnityEngine;

public class AddItemToInvetroryAction : EventAction {
    [SerializeField] private ItemData itemToGive;
    public override void ExecuteAction() {
        PlayerInventory.Instance.AddItem(itemToGive);
    }
}
