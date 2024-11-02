using UnityEngine;

public class TimerEventController : EventControllerBase {

    [Header("Actions and Conditions")]
    [SerializeField] private EventAction[] eventActions;
    [SerializeField] private EventCondition[] eventConditions;

    [Header("Timer")]
    [SerializeField] private float timeToEvent;

    private float timeElapsed = 0f;
    private bool eventIsTriggered;

    private void Update() {
        if (eventIsTriggered) {
            return;
        }
        if (timeElapsed < timeToEvent) {
            timeElapsed += Time.deltaTime;
        } else {
            eventIsTriggered = true;

            bool conditionMet = CheckConditions(eventConditions);
            if (conditionMet) {
                eventIsTriggered = true;

                foreach (EventAction action in eventActions) {
                    action.ExecuteEvent();
                }
            }
        }
    }
}
