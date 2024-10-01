using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utils;

public class SceneStateManager : MonoBehaviour {
    public static SceneStateManager Instance { get; private set; }

    public List<int> batteriesCollected { get; private set; } = new List<int>();
    public List<string> keysCollected { get; private set; } = new List<string>();

    public List<string> checkpoints { get; private set; } = new List<string>();

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
    public void CollectKey(string id) {
        keysCollected.Add(id);
    }
    public void AddCheckpoint(string id) {
        checkpoints.Add(id);
    }
}
