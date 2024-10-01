using UnityEngine;

public class WorldItem : Interactable {
    [SerializeField] private ItemData itemData;
    [SerializeField] private string pickUpMessage;
    public string itemId { get => itemData.itemName; }
    private void Start() {
        interactMessage = "Take";
        if (pickUpMessage.Length == 0) {
            pickUpMessage = $"Picked up {itemData.itemName}";
        }
    }
    protected override void Interact() {
        PlayerInventory.Instance.AddItem(itemData);
        TooltipUI.Instance.Show(pickUpMessage);
        SceneStateManager.Instance.CollectKey(itemData.itemName);
        Destroy(gameObject);
    }
}
