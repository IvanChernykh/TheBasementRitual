using UnityEngine;

public class FlashlightItem : Interactable {
    private void Start() {
        interactMessage = "Take";
    }
    protected override void Interact() {
        PlayerInventory.Instance.SetHasFlashlight(true);
        Flashlight.Instance.Equip();
        Destroy(gameObject);
    }
}
