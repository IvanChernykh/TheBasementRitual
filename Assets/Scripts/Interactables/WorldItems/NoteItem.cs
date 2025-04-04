using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Localization;

public class NoteItem : Interactable {
    [SerializeField] private NoteData noteData;
    private LocalizedString localizedNoteText;

    private void Start() {
        interactMessage = "Read";
        localizedNoteText = new LocalizedString {
            TableReference = LocalizationTables.Notes,
            TableEntryReference = noteData.localizationKey
        };
    }
    protected override void Interact() {
        localizedNoteText.StringChanged += OnNoteTextChanged;
        localizedNoteText.RefreshString();
    }

    private void OnNoteTextChanged(string localizedText) {
        GameUI.Notes.Show(localizedText);
        localizedNoteText.StringChanged -= OnNoteTextChanged;
    }
}
