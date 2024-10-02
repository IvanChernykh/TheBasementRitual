using System;
using UnityEngine.SceneManagement;

[Serializable]
public class SceneData {
    public int[] batteriesCollected;
    public string[] keysCollected;

    public string[] checkpoints;
    public DoorState[] doors;

    public GameScenes scene;
    public SceneData() {
        scene = GetGameSceneFromName(SceneManager.GetActiveScene().name);

        batteriesCollected = SceneStateManager.Instance.batteriesCollected.ToArray();
        keysCollected = SceneStateManager.Instance.keysCollected.ToArray();

        checkpoints = SceneStateManager.Instance.checkpoints.ToArray();
        doors = SceneStateManager.Instance.doors.ToArray();
    }

    private GameScenes GetGameSceneFromName(string sceneName) {
        if (Enum.TryParse(sceneName, out GameScenes parsedScene)) {
            return parsedScene;
        } else {
            return GameScenes.BasementLevel;
        }
    }
}
