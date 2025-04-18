using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

public enum SaveFileName {
    DefaultSave
}

public static class SaveSystem {
    private static readonly string fileExtention = ".save";
    public static async Task SaveGameAsync(SaveFileName? fileName = null, bool showSaveUI = false) {
        if (showSaveUI) {
            GameUI.SavingText.Show();
        }
        string saveFileName = fileName.HasValue ? fileName.ToString() : SaveFileName.DefaultSave.ToString();
        string path = Application.persistentDataPath + "/" + saveFileName + fileExtention;

        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(path, FileMode.Create)) {
            SaveData data = new SaveData();
            await Task.Run(() => formatter.Serialize(stream, data));
        }
    }
    public static void SaveGame(SaveFileName? fileName = null, bool showSaveUI = false) {
        if (showSaveUI) {
            GameUI.SavingText.Show();
        }
        string saveFileName = fileName.HasValue ? fileName.ToString() : SaveFileName.DefaultSave.ToString();
        string path = Application.persistentDataPath + "/" + saveFileName + fileExtention;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();
        formatter.Serialize(stream, data);

        stream.Close();
    }
    public static SaveData LoadSaveFile(SaveFileName fileName) {
        string path = Application.persistentDataPath + "/" + fileName.ToString() + fileExtention;

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
    public static bool SaveFileExists(SaveFileName fileName) {
        string path = Application.persistentDataPath + "/" + fileName.ToString() + fileExtention;
        return File.Exists(path);
    }
}
