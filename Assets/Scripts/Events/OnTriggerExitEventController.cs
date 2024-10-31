using System.Collections;
using UnityEngine;

public class OnTriggerExitEventController : MonoBehaviour {
    [SerializeField] private bool triggerAlways;
    [SerializeField] private bool destroySelfAfterEvent;
    [SerializeField] private EventAction[] eventActions;
    [SerializeField] private EventCondition[] eventConditions;

    private bool eventIsTriggered;

    private void OnTriggerExit(Collider other) {
        if (!eventIsTriggered || triggerAlways) {
            if (other.CompareTag("Player")) {
                bool conditionMet = CheckConditions();
                if (conditionMet) {
                    eventIsTriggered = true;

                    foreach (EventAction action in eventActions) {
                        action.ExecuteEvent();
                    }
                    if (destroySelfAfterEvent) {
                        StartCoroutine(DestroySelf());
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
    private IEnumerator DestroySelf() {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
