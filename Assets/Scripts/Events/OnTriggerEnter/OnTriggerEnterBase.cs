using UnityEngine;

public class OnTriggerEnterBase : MonoBehaviour {
    protected bool eventIsTriggered;
    private void OnTriggerEnter(Collider other) {
        if (!eventIsTriggered) {
            if (other.CompareTag("Player")) {
                eventIsTriggered = true;
                HandleEvent();
            }
        }
    }
    protected virtual void HandleEvent() {
        Debug.LogWarning("OnTriggerEnterBase.HandleEvent();");
    }
}
