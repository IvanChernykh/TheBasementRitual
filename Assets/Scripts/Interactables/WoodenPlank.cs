using Assets.Scripts.Utils;
using UnityEngine;

public class WoodenPlank : Interactable {
    [SerializeField] private ItemData hammer;
    [SerializeField] private BlockedDoor door;
    [SerializeField] private AudioClip removeSFX;

    private void Start() {
        interactMessage = "Remove";
    }
    protected override void Interact() {
        if (PlayerInventory.Instance.HasItem(hammer)) {
            SoundManager.Instance.PlaySound(removeSFX, transform.position, volume: .5f, pitch: Random.Range(1.2f, 1.8f));
            door.RemovePlank();
            Destroy(gameObject);
        } else {
            TooltipUI.Instance.Show(LocalizationHelper.LocalizeTooltip("No items"));
        }
    }
}
