using UnityEngine;

public class WorldItem : Interactable {
    [SerializeField] private ItemData itemData;
    [SerializeField] private string pickUpMessage;
    private void Start() {
        interactMessage = "Take";
        if (pickUpMessage.Length == 0) {
            pickUpMessage = $"Picked up {itemData.itemName}";
        }
        if (SceneStateManager.Instance.keysCollected.Exists(item => item == itemData.itemName)) {
            Destroy(gameObject);
        }
    }
    protected override void Interact() {
        PlayerInventory.Instance.AddItem(itemData);
        TooltipUI.Instance.Show(pickUpMessage);
        SceneStateManager.Instance.CollectKey(itemData.itemName);
        Destroy(gameObject);
    }
}
