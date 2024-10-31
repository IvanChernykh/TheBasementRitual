using UnityEngine;

public class RitualBook : MonoBehaviour {
    [SerializeField] private GameObject bookInteractable;
    [SerializeField] private GameObject bookStatic;
    private int saltCount;
    private readonly int saltCountMax = 4;
    private bool bookActivated;

    private void Update() {
        if (!bookActivated && saltCount == saltCountMax) {
            ActivateBook();
        }
    }
    private void ActivateBook() {
        bookActivated = true;
        bookInteractable.SetActive(true);
        bookStatic.SetActive(false);
    }

    public void PourSalt() {
        saltCount += 1;
    }
}
