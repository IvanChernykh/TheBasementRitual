using UnityEngine;

public class HistoryBookItem : MonoBehaviour {
    [SerializeField] private ItemData data;

    public ItemData bookData { get => data; }
}
