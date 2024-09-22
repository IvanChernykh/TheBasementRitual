using UnityEngine;

public class ExecuteAnotherAction : EventAction {
    [SerializeField] private bool triggerAlways;
    [SerializeField] private EventAction[] eventActions;
    [SerializeField] private EventCondition[] eventConditions;

    private bool eventIsTriggered;

    public override void ExecuteEvent() {
        if (!eventIsTriggered || triggerAlways) {
            if (ConditionsMet()) {
                eventIsTriggered = true;
                foreach (EventAction action in eventActions) {
                    action.ExecuteEvent();
                }
            }
        }
    }
    private bool ConditionsMet() {
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
