using System;
using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

public class HistoryBooks : MonoBehaviour {
    public static HistoryBooks Instance { get; private set; }

    [Header("Books")]
    [SerializeField] private HistoryBookItem[] books;
    [SerializeField] private ItemData[] neededBooks;
    [SerializeField] private Material defaultMat;
    [SerializeField] private Material highlightMat;

    private readonly float animationDuration = .25f;

    [Header("Interaction Triggers")]
    [SerializeField] private GameObject placeBookTrigger;
    [SerializeField] private GameObject arrangeBooksTrigger;

    [Header("Actions")]
    [SerializeField] private EventAction[] allBooksPlacedActions;

    private bool bookPlaced;

    // arrange books
    private bool isArranging = false;
    private int selectedBookIndex = 0;
    private int firstSelectedBookIndex = -1;

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
            // if (CheckNeededBooks()) {
            Destroy(placeBookTrigger);
            foreach (EventAction action in allBooksPlacedActions) {
                action.ExecuteEvent();
            }
            arrangeBooksTrigger.SetActive(true);
            // }
        }
        if (isArranging) {
            HighlightSelectedBook();
            HandleBookSelection();
        }
    }

    // place needed books
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

    // arrange books
    public void StartArrangeBooks() {
        Debug.Log("start");
        isArranging = true;
        arrangeBooksTrigger.SetActive(false);
        PlayerController.Instance.DisableCharacterController();
    }
    public void StopArrangeBooks() {
        isArranging = false;
        arrangeBooksTrigger.SetActive(true);
        PlayerController.Instance.EnableCharacterController();
    }

    private void HighlightSelectedBook() {
        for (int i = 0; i < books.Length; i++) {
            if (i == selectedBookIndex) {
                books[i].gameObject.GetComponent<MeshRenderer>().material = highlightMat;
            } else {
                books[i].gameObject.GetComponent<MeshRenderer>().material = defaultMat;
            }
        }
    }

    private void HandleBookSelection() {
        if (Input.GetKeyDown(KeyCode.A)) {
            selectedBookIndex = Mathf.Max(0, selectedBookIndex - 1);
            HighlightSelectedBook();
        } else if (Input.GetKeyDown(KeyCode.D)) {
            selectedBookIndex = Mathf.Min(books.Length - 1, selectedBookIndex + 1);
            HighlightSelectedBook();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (firstSelectedBookIndex == -1) {
                // Якщо жодна книга ще не вибрана, вибираємо першу
                SelectFirstBook();
            } else if (firstSelectedBookIndex == selectedBookIndex) {
                // Якщо гравець натискає E на вже вибраній книзі, повертаємо її назад
                StartCoroutine(PullBookBack(books[firstSelectedBookIndex].transform));
                firstSelectedBookIndex = -1; // Скидаємо вибір
            } else {
                // Якщо вибрана інша книга, обмінюємо місцями
                StartCoroutine(SwapBooksRoutine());
            }
        }
    }
    private void SelectFirstBook() {
        firstSelectedBookIndex = selectedBookIndex;
        StartCoroutine(PushBookForward(books[firstSelectedBookIndex].transform)); // Висуваємо книгу вперед
    }

    private IEnumerator SwapBooksRoutine() {
        Transform firstBook = books[firstSelectedBookIndex].transform;
        Transform secondBook = books[selectedBookIndex].transform;

        // Висуваємо другу книгу вперед
        yield return StartCoroutine(PushBookForward(secondBook));

        // Міняємо місцями книги
        yield return StartCoroutine(SwapBookPositions(firstBook, secondBook));

        // Засуваємо обидві книги назад
        yield return StartCoroutine(PullBothBooksBack(firstBook, secondBook));

        // Міняємо книги місцями в масиві
        SwapBooksInArray(firstSelectedBookIndex, selectedBookIndex);
        selectedBookIndex = firstSelectedBookIndex;
        firstSelectedBookIndex = -1; // Скидаємо вибір
    }
    private IEnumerator PullBothBooksBack(Transform firstBook, Transform secondBook) {
        Vector3 firstBookForwardPos = firstBook.position;
        Vector3 secondBookForwardPos = secondBook.position;

        Vector3 firstBookStartPos = firstBookForwardPos - Vector3.forward * 0.1f;
        Vector3 secondBookStartPos = secondBookForwardPos - Vector3.forward * 0.1f;

        float duration = animationDuration;
        float elapsedTime = 0f;

        // Одночасне засування обох книг
        while (elapsedTime < duration) {
            firstBook.position = Vector3.Lerp(firstBookForwardPos, firstBookStartPos, elapsedTime / duration);
            secondBook.position = Vector3.Lerp(secondBookForwardPos, secondBookStartPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        firstBook.position = firstBookStartPos;
        secondBook.position = secondBookStartPos;
    }

    private IEnumerator PushBookForward(Transform book) {
        Vector3 startPos = book.position;
        Vector3 forwardPos = startPos + Vector3.forward * 0.1f;
        float duration = animationDuration;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            book.position = Vector3.Lerp(startPos, forwardPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        book.position = forwardPos;
    }

    private IEnumerator PullBookBack(Transform book) {
        Vector3 forwardPos = book.position;
        Vector3 startPos = forwardPos - Vector3.forward * 0.1f;
        float duration = animationDuration;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            book.position = Vector3.Lerp(forwardPos, startPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        book.position = startPos;
    }

    private IEnumerator SwapBookPositions(Transform firstBook, Transform secondBook) {
        Vector3 firstBookStartPos = firstBook.position;
        Vector3 secondBookStartPos = secondBook.position;
        float duration = animationDuration;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            firstBook.position = Vector3.Lerp(firstBookStartPos, secondBookStartPos, elapsedTime / duration);
            secondBook.position = Vector3.Lerp(secondBookStartPos, firstBookStartPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        firstBook.position = secondBookStartPos;
        secondBook.position = firstBookStartPos;
    }

    private void SwapBooksInArray(int indexA, int indexB) {
        var temp = books[indexA];
        books[indexA] = books[indexB];
        books[indexB] = temp;
    }
}
