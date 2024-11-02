using System.Collections;
using UnityEngine;


public class OnTriggerEnterEventController : EventControllerBase {
    [SerializeField] private bool triggerAlways;
    [SerializeField] private bool destroySelfAfterEvent;
    [SerializeField] private EventAction[] eventActions;
    [SerializeField] private EventCondition[] eventConditions;

    private bool eventIsTriggered;

    private void OnTriggerEnter(Collider other) {
        if (!eventIsTriggered || triggerAlways) {
            if (other.CompareTag("Player")) {
                bool conditionMet = CheckConditions(eventConditions);
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

    private IEnumerator DestroySelf() {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
