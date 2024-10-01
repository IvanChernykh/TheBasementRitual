using System;
using TMPro;
using UnityEngine;
using Assets.Scripts.Utils;

public class NotesUI : MonoBehaviour {
    public static NotesUI Instance { get; private set; }
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI noteText;
    [SerializeField] private GameObject postProcessingBlur;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
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
