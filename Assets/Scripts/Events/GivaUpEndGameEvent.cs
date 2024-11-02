using UnityEngine;

public class GivaUpEndGameEvent : MonoBehaviour {
    [Header("Actions")]
    [SerializeField, Tooltip("Action when time is out")] private EventAction[] endGameActions;

    [Header("Timer")]
    [SerializeField] private float timeToEndGame = 600f;
    private float timePassed = 0f;

    private bool inRoom = false;
    private bool leftRoom = false;
    private bool timeIsOut = false;

    private void Update() {
        if (timeIsOut) {
            return;
        }
        if (timePassed >= timeToEndGame) {
            timeIsOut = true;
            foreach (EventAction action in endGameActions) {
                action.ExecuteEvent();
            }
            return;
        }
        if (inRoom && !leftRoom) {
            timePassed += Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            inRoom = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            inRoom = false;
            leftRoom = true;
        }
    }
}
