using UnityEngine;

public class WorldItem : Interactable {
    [SerializeField] private ItemData itemData;
    [SerializeField] private string pickUpMessage;
    private void Start() {
        interactMessage = "Take";
        if (pickUpMessage.Length == 0) {
            pickUpMessage = $"Picked up {itemData.itemName}";
        }
    }
    public override void Interact() {
        PlayerInventory.Instance.AddItem(itemData);
        TooltipUI.Instance.Show(pickUpMessage);
        Destroy(gameObject);
    }
}
