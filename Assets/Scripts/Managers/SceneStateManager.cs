using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utils;
using System;

[Serializable]
public class EventData {
    public int id;
    public bool executeOnLoad;

    public EventData(int id, bool executeOnLoad) {
        this.id = id;
        this.executeOnLoad = executeOnLoad;
    }
}
public class SceneStateManager : MonoBehaviour {
    public static SceneStateManager Instance { get; private set; }

    public List<EventData> eventsTriggered { get; private set; } = new List<EventData>();
    public List<int> batteriesCollected { get; private set; } = new List<int>();
    public List<string> keysCollected { get; private set; } = new List<string>();
    public List<int> checkpoints { get; private set; } = new List<int>();

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void CollectBattery(int id) {
        batteriesCollected.Add(id);
    }
    public void CollectKey(string key) {
        keysCollected.Add(key);
    }
    public void AddEvent(EventData data) {
        eventsTriggered.Add(data);
    }
    public void AddCheckpoint(int id) {
        checkpoints.Add(id);
    }
}
