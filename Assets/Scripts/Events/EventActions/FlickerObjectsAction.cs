using System.Collections;
using UnityEngine;

public class FlickerObjectsAction : EventAction {
    private enum ObjectToKeep {
        object1 = 1,
        object2 = 2
    }
    [Header("Delay")]
    [SerializeField] private float delay = 0f;
    [Header("Settings")]
    [SerializeField] private GameObject object1;
    [SerializeField] private GameObject object2;
    [SerializeField] private float flickerMinTime = 0.1f;
    [SerializeField] private float flickerMaxTime = 0.9f;
    [SerializeField] private float totalTime = 5f;
    [SerializeField] private ObjectToKeep objectToKeep = ObjectToKeep.object1;

    private Coroutine flickerCoroutine;

    public override void ExecuteAction() {
        if (delay > 0) {
            StartCoroutine(StartFlickerWithDelay());
        } else {
            StartFlicker();
        }
    }
    private IEnumerator StartFlickerWithDelay() {
        yield return new WaitForSeconds(delay);
        StartFlicker();
    }

    private void StartFlicker() {
        flickerCoroutine = StartCoroutine(FlickerObjects());
    }

    private void StopFlicker() {
        if (flickerCoroutine != null) {
            StopCoroutine(flickerCoroutine);
        }
        if ((int)objectToKeep == 1) {
            object1.SetActive(true);
            object2.SetActive(false);
        } else {
            object1.SetActive(false);
            object2.SetActive(true);
        }
    }

    private IEnumerator FlickerObjects() {
        float elapsedTime = 0f;

        while (elapsedTime < totalTime) {
            float waitTime = Random.Range(flickerMinTime, flickerMaxTime);
            object1.SetActive(!object1.activeSelf);
            object2.SetActive(!object2.activeSelf);

            yield return new WaitForSeconds(waitTime);
            elapsedTime += waitTime;
        }
        StopFlicker();
    }
}
