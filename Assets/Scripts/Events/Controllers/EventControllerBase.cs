using UnityEngine;

public abstract class EventControllerBase : MonoBehaviour {
    protected bool CheckConditions(EventCondition[] eventConditions) {
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
