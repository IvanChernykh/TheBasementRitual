using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    public string interactMessage { get; protected set; }
    [SerializeField] private EventAction[] interactEventAction;
    [SerializeField] private bool executeEventActionAlways;
    private bool eventExecuted;
    abstract protected void Interact();
    public void InteractAction() {
        Interact();
        if (!eventExecuted || executeEventActionAlways) {
            eventExecuted = true;
            foreach (EventAction item in interactEventAction) {
                item.ExecuteEvent();
            }
        }
    }
}
