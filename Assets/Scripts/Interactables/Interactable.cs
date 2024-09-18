using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    public string interactMessage { get; protected set; }
    [SerializeField] private EventAction[] interactEventAction;
    abstract protected void Interact();
    public void InteractAction() {
        Interact();
        foreach (EventAction item in interactEventAction) {
            item.ExecuteEvent();
        }
    }
}
