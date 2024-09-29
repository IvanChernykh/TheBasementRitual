
using System;

[Serializable]
public class SceneData {
    public int[] batteriesCollected;
    public SceneData() {
        batteriesCollected = SceneStateManager.Instance.batteriesCollected.ToArray();
    }
}
