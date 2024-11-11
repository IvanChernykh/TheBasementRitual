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

    private readonly float bookAnimationDuration = .25f;
    private readonly float bookOffset = .1f;

    [Header("Book Combination")]
    [SerializeField] private HistoryBookItem[] booksCombo;

    [Header("Interaction Triggers")]
    [SerializeField] private GameObject placeBookTrigger;
    [SerializeField] private GameObject arrangeBooksTrigger;

    [Header("Actions")]
    [SerializeField] private EventAction[] allBooksPlacedActions;
    [SerializeField] private EventAction hiddenDoorAction;

    // placing books
    private bool bookPlaced;

    // arrange books
    private bool isArranging = false;
    private bool canMoveBook = false;
    private int selectedBookIndex = 0;
    private int firstSelectedBookIndex = -1;

    // puzzle solved
    public bool puzzleSolved { get; private set; } = false;



    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update() {
        if (puzzleSolved) {
            return;
        }
        if (bookPlaced) {
            bookPlaced = false;
            if (CheckNeededBooks()) {
                Destroy(placeBookTrigger);
                foreach (EventAction action in allBooksPlacedActions) {
                    action.ExecuteEvent();
                }
                arrangeBooksTrigger.SetActive(true);
            }
        }
        if (isArranging) {
            puzzleSolved = CheckCombination();

            if (puzzleSolved) {
                OnPuzzleSolved();
                return;
            }
            HighlightSelectedBook();
            HandleBookSelection();
        }
    }

    // place needed books
    public void TryPlaceBook() {
        ItemData playerBook = PlayerInventory.Instance.items.Find(item => Array.Exists(neededBooks, nb => nb == item));

        if (playerBook == null) {
            TooltipUI.Instance.Show(LocalizationHelper.LocalizeTooltip("NoBooks"));
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
        isArranging = true;
        canMoveBook = true;
        arrangeBooksTrigger.SetActive(false);
        PlayerController.Instance.DisableCharacterController();
        TooltipUI.Instance.ShowAlways("BookTooltip");
    }
    public void StopArrangeBooks() {
        isArranging = false;
        canMoveBook = false;
        arrangeBooksTrigger.SetActive(true);
        books[selectedBookIndex].gameObject.GetComponent<MeshRenderer>().material = defaultMat;
        PlayerController.Instance.EnableCharacterController();
        TooltipUI.Instance.Hide();
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
        if (!canMoveBook) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (firstSelectedBookIndex != -1) {
                StartCoroutine(PullBookBack(books[firstSelectedBookIndex].transform));
                firstSelectedBookIndex = -1;
            }
            StopArrangeBooks();
            return;
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            selectedBookIndex = Mathf.Max(0, selectedBookIndex - 1);
            HighlightSelectedBook();
        } else if (Input.GetKeyDown(KeyCode.D)) {
            selectedBookIndex = Mathf.Min(books.Length - 1, selectedBookIndex + 1);
            HighlightSelectedBook();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            canMoveBook = false;
            if (firstSelectedBookIndex == -1) {
                SelectFirstBook();
            } else if (firstSelectedBookIndex == selectedBookIndex) {
                StartCoroutine(PullBookBack(books[firstSelectedBookIndex].transform));
                firstSelectedBookIndex = -1;
            } else {
                StartCoroutine(SwapBooksRoutine());
            }
        }
    }
    private void SelectFirstBook() {
        firstSelectedBookIndex = selectedBookIndex;
        StartCoroutine(PushBookForward(books[firstSelectedBookIndex].transform, canMoveOnEnd: true));
    }

    private IEnumerator SwapBooksRoutine() {
        Transform firstBook = books[firstSelectedBookIndex].transform;
        Transform secondBook = books[selectedBookIndex].transform;

        yield return StartCoroutine(PushBookForward(secondBook));
        yield return StartCoroutine(SwapBookPositions(firstBook, secondBook));
        yield return StartCoroutine(PullBothBooksBack(firstBook, secondBook));

        SwapBooksInArray(firstSelectedBookIndex, selectedBookIndex);
        selectedBookIndex = firstSelectedBookIndex;
        firstSelectedBookIndex = -1;
        canMoveBook = true;
    }
    private IEnumerator PullBothBooksBack(Transform firstBook, Transform secondBook) {
        Vector3 firstBookForwardPos = firstBook.position;
        Vector3 secondBookForwardPos = secondBook.position;

        Vector3 firstBookStartPos = firstBookForwardPos - Vector3.forward * bookOffset;
        Vector3 secondBookStartPos = secondBookForwardPos - Vector3.forward * bookOffset;

        float elapsedTime = 0f;

        while (elapsedTime < bookAnimationDuration) {
            firstBook.position = Vector3.Lerp(firstBookForwardPos, firstBookStartPos, elapsedTime / bookAnimationDuration);
            secondBook.position = Vector3.Lerp(secondBookForwardPos, secondBookStartPos, elapsedTime / bookAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        firstBook.position = firstBookStartPos;
        secondBook.position = secondBookStartPos;
    }

    private IEnumerator PushBookForward(Transform book, bool canMoveOnEnd = false) {
        Vector3 startPos = book.position;
        Vector3 forwardPos = startPos + Vector3.forward * bookOffset;
        float elapsedTime = 0f;

        while (elapsedTime < bookAnimationDuration) {
            book.position = Vector3.Lerp(startPos, forwardPos, elapsedTime / bookAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        book.position = forwardPos;
        if (canMoveOnEnd) {
            canMoveBook = true;
        }
    }

    private IEnumerator PullBookBack(Transform book) {
        Vector3 forwardPos = book.position;
        Vector3 startPos = forwardPos - Vector3.forward * bookOffset;
        float elapsedTime = 0f;

        while (elapsedTime < bookAnimationDuration) {
            book.position = Vector3.Lerp(forwardPos, startPos, elapsedTime / bookAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        book.position = startPos;
        canMoveBook = true;
    }

    private IEnumerator SwapBookPositions(Transform firstBook, Transform secondBook) {
        Vector3 firstBookStartPos = firstBook.position;
        Vector3 secondBookStartPos = secondBook.position;
        float elapsedTime = 0f;

        while (elapsedTime < bookAnimationDuration) {
            firstBook.position = Vector3.Lerp(firstBookStartPos, secondBookStartPos, elapsedTime / bookAnimationDuration);
            secondBook.position = Vector3.Lerp(secondBookStartPos, firstBookStartPos, elapsedTime / bookAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        firstBook.position = secondBookStartPos;
        secondBook.position = firstBookStartPos;
    }

    private void SwapBooksInArray(int indexA, int indexB) {
        HistoryBookItem temp = books[indexA];
        books[indexA] = books[indexB];
        books[indexB] = temp;
    }

    // combination
    private bool CheckCombination() {
        bool solved = true;
        for (int i = 0; i < books.Length; i++) {
            if (books[i].bookData != booksCombo[i].bookData) {
                solved = false;
            }
        }
        return solved;
    }

    // puzzle solved
    private void OnPuzzleSolved() {
        StopArrangeBooks();
        Destroy(arrangeBooksTrigger);
        hiddenDoorAction.ExecuteEvent();
    }
}
