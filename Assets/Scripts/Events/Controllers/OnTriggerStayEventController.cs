using UnityEngine;

public class OnTriggerStayEventController : MonoBehaviour {
    [SerializeField] private EventAction[] actionsOnEnter;
    [SerializeField] private EventAction[] actionsOnExit;
    [SerializeField] private bool executeAlways;

    private bool enterIsTriggered;
    private bool exitIsTriggered;
    private void OnTriggerEnter(Collider other) {
        if (executeAlways) {
            if (other.CompareTag("Player")) {
                ExecuteActions(actionsOnEnter);
            }
        } else {
            if (!enterIsTriggered) {
                if (other.CompareTag("Player")) {
                    enterIsTriggered = true;
                    ExecuteActions(actionsOnEnter);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (executeAlways) {
            if (other.CompareTag("Player")) {
                ExecuteActions(actionsOnExit);
            }
        } else {
            if (!exitIsTriggered) {
                if (other.CompareTag("Player")) {
                    exitIsTriggered = true;
                    ExecuteActions(actionsOnExit);
                }
            }
        }
    }
    private void ExecuteActions(EventAction[] actions) {
        foreach (EventAction action in actions) {
            action.ExecuteEvent();
        }
    }
}
