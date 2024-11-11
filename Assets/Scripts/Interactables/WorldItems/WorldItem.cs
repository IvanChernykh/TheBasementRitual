using Assets.Scripts.Utils;
using UnityEngine;

public class WorldItem : Interactable {
    [SerializeField] private ItemData itemData;
    [SerializeField] private string pickUpMessage;
    public string ItemId { get => itemData.itemName; }

    private bool tooltipIsLocalized;

    private void Start() {
        interactMessage = "Take";
        if (pickUpMessage.Length == 0) {
            pickUpMessage = LocalizationHelper.LocalizeTooltip("PickedUp", itemData.itemName);
            tooltipIsLocalized = true;
        }
    }
    protected override void Interact() {
        PlayerInventory.Instance.AddItem(itemData);
        TooltipUI.Instance.Show(tooltipIsLocalized ? pickUpMessage : LocalizationHelper.LocalizeTooltip(pickUpMessage));
        SceneStateManager.Instance.CollectKey(itemData.itemName);
        Destroy(gameObject);
    }
}
