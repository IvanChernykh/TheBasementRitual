using UnityEngine;


public class OnTriggerEnterEventController : MonoBehaviour {
    [SerializeField] private bool triggerAlways;
    [SerializeField] private EventAction[] eventActions;
    [SerializeField] private EventCondition[] eventConditions;

    private bool eventIsTriggered;
    private void OnTriggerEnter(Collider other) {
        if (!eventIsTriggered || triggerAlways) {
            if (other.CompareTag("Player")) {
                Debug.Log(123123123123);
                bool conditionMet = CheckConditions();
                if (conditionMet) {
                    eventIsTriggered = true;
                    foreach (EventAction action in eventActions) {
                        action.ExecuteEvent();
                    }
                }
            }
        }
    }
    private bool CheckConditions() {
        bool conditionMet = true;
        foreach (EventCondition condition in eventConditions) {
            if (!condition.IsConditionMet()) {
                conditionMet = false;
                break;
            }
        }
        return conditionMet;
    }
}
