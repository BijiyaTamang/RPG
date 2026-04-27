// SaveSystem.cs
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    const string FILE_NAME = "save.json";

    // Application.persistentDataPath is the correct cross-platform
    // path. It works on PC, Mac, mobile, and consoles.
    static string SavePath =>
        Path.Combine(Application.persistentDataPath, FILE_NAME);

    public static void Save(PlayerSaveData data)
    {
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[SaveSystem] Saved to: {SavePath}");
    }

    public static PlayerSaveData Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("[SaveSystem] No save found. Creating new.");
            return new PlayerSaveData(); // default values
        }

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<PlayerSaveData>(json);
    }

    public static void Delete()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }

    public static bool HasSave() => File.Exists(SavePath);
}