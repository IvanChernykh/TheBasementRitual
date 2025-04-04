using TMPro;
using UnityEngine;

public class NotesUI : MonoBehaviour {

    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI noteText;
    [SerializeField] private GameObject postProcessingBlur;

    private void Start() {
        Hide();
    }

    public void Show(string text) {
        GameStateManager.Instance.EnterReadingNoteState();

        postProcessingBlur.SetActive(true);
        container.SetActive(true);
        noteText.text = text;
    }
    public void Hide() {
        postProcessingBlur.SetActive(false);
        container.SetActive(false);
        noteText.text = "";
    }
}
