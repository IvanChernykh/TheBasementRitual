using System;
using Assets.Scripts.Utils;
using UnityEngine;

public class HistoryBooks : MonoBehaviour {
    public static HistoryBooks Instance { get; private set; }

    [Header("Books")]
    [SerializeField] private HistoryBookItem[] books;
    [SerializeField] private ItemData[] neededBooks;

    [Header("Interaction Triggers")]
    [SerializeField] private GameObject placeBookTrigger;

    [Header("Actions")]
    [SerializeField] private EventAction[] allBooksPlacedActions;

    private bool bookPlaced;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update() {
        if (bookPlaced) {
            bookPlaced = false;
            if (CheckNeededBooks()) {
                Destroy(placeBookTrigger);
                foreach (EventAction action in allBooksPlacedActions) {
                    action.ExecuteEvent();
                }
            }
        }

    }

    public void TryPlaceBook() {
        ItemData playerBook = PlayerInventory.Instance.items.Find(item => Array.Exists(neededBooks, nb => nb == item));

        if (playerBook == null) {
            return;
        }
        foreach (HistoryBookItem elem in books) {
            if (elem.bookData == playerBook) {
                PlayerInventory.Instance.RemoveItem(playerBook);
                elem.gameObject.SetActive(true);
                bookPlaced = true;
                break;
            }
        }
    }
    public bool CheckNeededBooks() {
        bool allBooksPlaced = true;
        foreach (HistoryBookItem elem in books) {
            if (!elem.gameObject.activeSelf) {
                allBooksPlaced = false;
                break;
            }
        }
        return allBooksPlaced;
    }
}
