using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private SaveSystem saveSystem;

    private void Start()
    {
        saveSystem = GetComponent<SaveSystem>();
    }

    public void SaveGame()
    {
        PlayerData playerData = new PlayerData();
        /* Populate playerData with relevant data */

        saveSystem.SaveGame(playerData);
    }

    public void LoadGame()
    {
        PlayerData loadedData = saveSystem.LoadGame();

        if (loadedData != null)
        {
            /* Update game state using the loaded data */
        }
    }
}
