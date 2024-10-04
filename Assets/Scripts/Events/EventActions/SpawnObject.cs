using System.Collections;
using UnityEngine;

public class SpawnObject : EventAction {
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform positionToSpawn;
    [SerializeField] private float delay = 0f;

    public override void ExecuteEvent() {
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
