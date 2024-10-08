using UnityEngine;

public class Checkpoint : MonoBehaviour {
    [SerializeField] private string id;
    public string Id { get => id; }
    private bool eventIsTriggered;
    private void OnTriggerEnter(Collider other) {
        if (!eventIsTriggered) {
            if (other.CompareTag("Player")) {
                eventIsTriggered = true;
                SceneStateManager.Instance.AddCheckpoint(id);
                SaveSystem.SaveGame(showSaveUI: true);
            }
        }
    }
}
