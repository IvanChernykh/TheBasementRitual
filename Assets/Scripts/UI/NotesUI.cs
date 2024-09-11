using System;
using TMPro;
using UnityEngine;

public class NotesUI : MonoBehaviour {
    public static NotesUI Instance { get; private set; }
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI noteText;

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        Hide();
        InputManager.Instance.OnUnpause += OnUnpause;
    }

    public void Show(string text) {
        GameStateManager.Instance.SetReadingNoteState();

        container.SetActive(true);
        noteText.text = text;
    }
    public void Hide() {
        GameStateManager.Instance.SetPlayingState();

        container.SetActive(false);
        noteText.text = "";
    }
    private void OnUnpause(object sender, EventArgs e) {
        if (GameStateManager.Instance.IsReadingNote()) {
            Hide();
        }
    }
}
