using Assets.Scripts.Utils;
using UnityEngine;

public class BatteryItem : Interactable {
    // todo: make it string type
    [SerializeField] private int id;

    public int BatteryId { get => id; }

    private void Start() {
        interactMessage = "Take";
    }
    protected override void Interact() {
        PlayerInventory playerInventory = PlayerInventory.Instance;
        if (playerInventory.batteries < playerInventory.batteriesMax) {
            playerInventory.AddBattery();
            SceneStateManager.Instance.CollectBattery(id);
            TooltipUI.Instance.Show(LocalizationHelper.LocalizeTooltip("BatteryPickUp"));
            Destroy(gameObject);
        } else {
            TooltipUI.Instance.Show(LocalizationHelper.LocalizeTooltip("BatteriesMax"));
        }
    }
}
