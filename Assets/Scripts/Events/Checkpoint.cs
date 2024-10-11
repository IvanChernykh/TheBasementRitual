using UnityEngine;

public class Checkpoint : MonoBehaviour {
    [SerializeField] private EventCondition[] conditions;
    [SerializeField] private string id;
    public string Id { get => id; }
    private bool eventIsTriggered;

    private void OnTriggerEnter(Collider other) {
        if (!eventIsTriggered) {
            if (other.CompareTag("Player")) {
                bool conditionMet = CheckConditions();
                if (conditionMet) {
                    eventIsTriggered = true;
                    SceneStateManager.Instance.AddCheckpoint(id);
                    SaveSystem.SaveGame(showSaveUI: true);
                }
            }
        }
    }
    private bool CheckConditions() {
        bool conditionMet = true;
        foreach (EventCondition condition in conditions) {
            if (!condition.IsConditionMet()) {
                conditionMet = false;
                break;
            }
        }
        return conditionMet;
    }
}
