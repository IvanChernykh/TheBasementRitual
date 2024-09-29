using System;

[Serializable]
public class SaveData {
    public PlayerData playerData;
    // public SceneData sceneData;

    public SaveData() {
        playerData = new PlayerData();
        // sceneData = scene;
    }
}
