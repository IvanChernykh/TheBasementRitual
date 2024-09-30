
using System;

[Serializable]
public class SceneData {
    public int[] batteriesCollected;
    public string[] keysCollected;
    public EventData[] eventsTriggered;
    public int[] doorsOpened;
    public int[] checkpoints;
    public SceneData() {
        batteriesCollected = SceneStateManager.Instance.batteriesCollected.ToArray();
        keysCollected = SceneStateManager.Instance.keysCollected.ToArray();
        eventsTriggered = SceneStateManager.Instance.eventsTriggered.ToArray();
        checkpoints = SceneStateManager.Instance.checkpoints.ToArray();
    }
}
