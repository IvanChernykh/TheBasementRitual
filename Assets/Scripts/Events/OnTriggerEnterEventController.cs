using UnityEngine;


public class OnTriggerEnterEventController : MonoBehaviour {
    [SerializeField] private EventAction[] eventActions;

    private bool eventIsTriggered;
    private void OnTriggerEnter(Collider other) {
        if (!eventIsTriggered) {
            if (other.CompareTag("Player")) {
                eventIsTriggered = true;
                foreach (EventAction action in eventActions) {
                    action.ExecuteEvent();
                }
            }
        }
    }
}
