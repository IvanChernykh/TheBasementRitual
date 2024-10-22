using System.Collections;
using UnityEngine;

public class SpawnObject : EventAction {
    [Header("Action Settings")]
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform positionToSpawn;
    [SerializeField] private float delay = 0f;

    public override void ExecuteAction() {
        if (objectToSpawn != null) {
            if (delay > 0) {
                StartCoroutine(ShowWithDelay());
            } else {
                Spawn();
            }
        }
    }
    private void Spawn() {
        Instantiate(objectToSpawn, positionToSpawn.position, positionToSpawn.rotation);
    }

    private IEnumerator ShowWithDelay() {
        yield return new WaitForSeconds(delay);
        Spawn();
    }
}
