using Assets.Scripts.Utils;
using UnityEditor.Search;
using UnityEngine;
using System.Linq;

public class AllKeys : MonoBehaviour {
    public static AllKeys Instance { get; private set; }
    [SerializeField] private ItemData[] items;
    public ItemData[] Items { get => items; }

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
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