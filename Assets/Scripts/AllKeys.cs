using UnityEngine;
using System.Linq;
using Assets;
public class AllKeys : Singleton<AllKeys> {

    [SerializeField] private ItemData[] items;
    public ItemData[] Items { get => items; }

    private void Awake() {
        InitializeSingleton();
    }

    public bool HasItem(string itemName) {
        return Items.Any(item => itemName == item.itemName);
    }
    public ItemData GetItem(string itemName) {
        foreach (ItemData item in Items) {
            if (item.itemName == itemName) {
                return item;
            }
        }
        return null;
    }
}