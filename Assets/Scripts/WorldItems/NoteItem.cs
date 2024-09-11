using UnityEngine;

public class NoteItem : Interactable {
    [SerializeField] private NoteData noteData;

    private void Start() {
        interactMessage = "Read";
    }
    public override void Interact() {
        // PlayerInventory.Instance.AddItem(itemData);
        Time.timeScale = 0;
        NotesUI.Instance.Show(noteData.noteText);
        Destroy(gameObject);
    }
}
