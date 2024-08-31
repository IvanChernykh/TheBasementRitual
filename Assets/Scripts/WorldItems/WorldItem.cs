using UnityEngine;

public class WorldItem : Interactable {
    [SerializeField] private ItemData itemData;
    private void Start() {
        interactMessage = "Take";
    }
    public override void Interact() {
        PlayerInventory.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
