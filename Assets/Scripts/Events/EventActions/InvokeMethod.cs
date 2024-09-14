using UnityEngine;
using UnityEngine.Events;

public class InvokeMethod : EventAction {
    [SerializeField] private UnityEvent eventMethod;
    public override void ExecuteEvent() {
        eventMethod?.Invoke();
    }
}
