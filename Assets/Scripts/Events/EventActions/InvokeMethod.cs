using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InvokeMethod : EventAction {
    [SerializeField] private UnityEvent eventMethod;
    [SerializeField] private float delay = 0f;
    public override void ExecuteEvent() {
        if (delay > 0) {
            StartCoroutine(InvokeWithDelay());
        } else {
            eventMethod?.Invoke();
        }
    }
    private IEnumerator InvokeWithDelay() {
        yield return new WaitForSeconds(delay);
        eventMethod?.Invoke();
    }
}
