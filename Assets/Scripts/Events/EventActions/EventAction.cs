using UnityEngine;

public abstract class EventAction : MonoBehaviour {
    [Header("Conditions")]
    [SerializeField] private EventCondition[] conditions;
    public abstract void ExecuteAction();

    public void ExecuteEvent() {
        if (CheckConditions()) {
            ExecuteAction();
        }
    }

    private bool CheckConditions() {
        bool conditionMet = true;

        if (conditions == null || conditions.Length == 0) {
            return conditionMet;
        }
        foreach (EventCondition condition in conditions) {
            if (!condition.IsConditionMet()) {
                conditionMet = false;
                break;
            }
        }
        return conditionMet;
    }
}
