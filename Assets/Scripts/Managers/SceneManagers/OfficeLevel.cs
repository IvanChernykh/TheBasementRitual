using UnityEngine;

public class OfficeLevel : MonoBehaviour {
    [SerializeField] private ItemData itemToRemove;

    private void Start() {
        if (PlayerInventory.Instance.HasItem(itemToRemove)) {
            PlayerInventory.Instance.RemoveItem(itemToRemove);
        }
    }
}
