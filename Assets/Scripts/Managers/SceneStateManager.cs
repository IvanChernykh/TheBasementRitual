using System.Collections.Generic;
using System;

[Serializable]
public class DoorState {
    public string id;
    public string lockedMessage;
    public string state;
    public DoorState(string id, string lockedMessage, string state) {
        this.id = id;
        this.lockedMessage = lockedMessage;
        this.state = state;
    }
}

public class SceneStateManager : Singleton<SceneStateManager> {

    public List<int> batteriesCollected { get; private set; } = new List<int>();
    public List<string> keysCollected { get; private set; } = new List<string>();

    public List<string> checkpoints { get; private set; } = new List<string>();
    public List<DoorState> doors { get; private set; } = new List<DoorState>();

    private void Awake() {
        InitializeSingleton();
    }

    public void CollectBattery(int id) {
        batteriesCollected.Add(id);
    }
    public void CollectKey(string id) {
        keysCollected.Add(id);
    }
    public void AddCheckpoint(string id) {
        checkpoints.Add(id);
    }
    public void AddOrUpdateDoorState(DoorState doorState) {
        for (int i = 0; i < doors.Count; i++) {
            if (doors[i].id == doorState.id) {
                doors[i] = doorState;
                return;
            }
        }
        doors.Add(doorState);
    }
}
