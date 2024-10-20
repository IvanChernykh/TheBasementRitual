using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    public string interactMessage { get; protected set; }
    [Header("Event Actions")]
    [SerializeField] private EventAction[] interactEventAction;
    [SerializeField] private EventCondition[] interactEventConditions;
    [Tooltip("Execute event action always"), SerializeField] private bool executeEventActionAlways;
    [Tooltip("Execute by condition only"), SerializeField] private bool executeByConditionOnly;
    [Tooltip("Execute before interaction"), SerializeField] private bool executeBeforeInteraction;
    private bool eventExecuted;
    abstract protected void Interact();
    public void InteractAction() {
        if (executeBeforeInteraction) {
            ExecuteEvent();
        }
        Interact();

        if (!executeBeforeInteraction) {
            ExecuteEvent();
        }
    }

    private void ExecuteEvent() {
        bool conditionMet = CheckConditions();
        if (executeByConditionOnly && conditionMet) {
            foreach (EventAction item in interactEventAction) {
                item.ExecuteEvent();
            }
            return;
        }
        if (!eventExecuted || executeEventActionAlways) {
            eventExecuted = true;

            if (conditionMet) {
                foreach (EventAction item in interactEventAction) {
                    item.ExecuteEvent();
                }
            }
        }
    }

    private bool CheckConditions() {
        bool conditionMet = true;
        if (interactEventConditions == null || interactEventConditions.Length == 0) {
            return conditionMet;
        }
        foreach (EventCondition condition in interactEventConditions) {
            if (!condition.IsConditionMet()) {
                conditionMet = false;
                break;
            }
        }
        return conditionMet;
    }
}
