using Assets.Scripts.Utils;
using UnityEngine;

public class BlockedDoor : MonoBehaviour {
    [SerializeField] private int planksToRemove = 1;
    [SerializeField] private Door door;

    private int planksRemoved = 0;
    private bool done;

    private void Update() {
        if (planksRemoved == planksToRemove && !done) {
            done = true;
            door.Unlock();
            Destroy(this);
            return;
        }
    }

    public void RemovePlank() {
        planksRemoved++;
    }
}
