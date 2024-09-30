using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utils;

public class SceneStateManager : MonoBehaviour {
    public static SceneStateManager Instance { get; private set; }
    public List<int> batteriesCollected { get; private set; } = new List<int>();
    public List<string> keysCollected { get; private set; } = new List<string>();

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
    }
    public void CollectBattery(int id) {
        batteriesCollected.Add(id);
    }
    public void CollectKey(string key) {
        keysCollected.Add(key);
        // todo: actually should remove key if door is opened
    }
}
