using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public void SaveGame(PlayerData data)
    {
        string json = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText("save.json", json);
    }

    public PlayerData LoadGame()
    {
        if (System.IO.File.Exists("save.json"))
        {
            string json = System.IO.File.ReadAllText("save.json");
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return null;
        }
    }
}
