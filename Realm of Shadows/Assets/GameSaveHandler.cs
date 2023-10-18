using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaveHandler : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private static Save CreateSaveGameObject()
    {
        Save save = new Save();
        save.gameState = GameData.gameState;
        save.playerMoney = GameData.playerMoney;
        save.playerXP = GameData.playerXP;
        save.unlockedFloors = GameData.unlockedFloors;
        save.totalDeaths = GameData.totalDeaths;
        save.moneySpent = GameData.moneySpent;
        save.totalSacrificed = GameData.totalSacrificed;

        return save;
    }

    public static void SaveGame()
    {

        Save save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();


        Debug.Log("Game Saved");
    }

    public static void LoadGame()
    {
        // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {

            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            GameData.gameState = save.gameState;
            GameData.playerMoney = save.playerMoney;
            GameData.playerXP = save.playerXP;
            GameData.unlockedFloors = save.unlockedFloors;
            GameData.totalDeaths = save.totalDeaths;
            GameData.moneySpent = save.moneySpent;
            GameData.totalSacrificed = save.totalSacrificed;

        }
        else
        {
            Debug.Log("Save game not found.");
        }
    }
}
