using UnityEngine;

public class NoteItem : Interactable {
    [SerializeField] private NoteData noteData;

    private void Start() {
        interactMessage = "Read";
    }
    public override void Interact() {
        // PlayerInventory.Instance.AddItem(itemData);
        NotesUI.Instance.Show(noteData.noteText);
        Destroy(gameObject);
    }
}
