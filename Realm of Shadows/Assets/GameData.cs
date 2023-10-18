using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    // Gamestate meanings:
    // 1 - after seeing the first cutscene
    // 2 - after beating floor one for the first time (unlocking Heart of the Cards)
    // 3 - after meeting Eyes (after beating floor 2 for the first time)
    // 4 - after seeing the Eyes contacting Chazz cutscene (after beating floor 3 for the first time)

    public static int gameState = 0; 
    public static int playerMoney = 0;
    public static int playerXP = 0;
    public static int unlockedFloors = 1;

    public static int totalDeaths = 0;
    public static int moneySpent = 0;
    public static int totalSacrificed = 0;
    

}
