
using System;

[Serializable]
public class SceneData {
    public int[] batteriesCollected;
    public string[] keysCollected;
    public SceneData() {
        batteriesCollected = SceneStateManager.Instance.batteriesCollected.ToArray();
        keysCollected = SceneStateManager.Instance.keysCollected.ToArray();
    }
}
