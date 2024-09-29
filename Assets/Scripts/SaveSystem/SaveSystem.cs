using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum SaveFileName {
    SaveGame1,
    SaveGame2,
    SaveGame3,
    DefaultSave
}

public static class SaveSystem {
    public static void SaveGame(SaveFileName? fileName = null) {

        string saveFileName = fileName.HasValue ? fileName.ToString() : SaveFileName.DefaultSave.ToString();
        string path = Application.persistentDataPath + "/" + saveFileName + ".save";
        Debug.Log(path);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();
        formatter.Serialize(stream, data);

        stream.Close();
    }
    public static SaveData LoadGame(SaveFileName fileName) {
        string path = Application.persistentDataPath + "/" + fileName.ToString() + ".save";

        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        } else {
            Debug.LogError("Save file not found: " + path);
            return null;
        }
    }
}
