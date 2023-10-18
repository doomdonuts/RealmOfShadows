using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    public int floorSize; // it's backwards from what you would think
    public RoomProperties[] floorLayout;
    public GameManager gameManager;
    public string[] allowedEnemies; // must match the prefab file name
    public string[] allowedObjects; // again, must match the prefab file name
    public int maxNumberOfEnemiesPerRoom;
    public int maxNumberOfItemsPerRoom;
    public int floorNumber;

    // Start is called before the first frame update
    void Start()
    {
        floorLayout = new RoomProperties[floorSize];

        GameObject startRoomGO = new GameObject();
        startRoomGO.name = "Room 0";

        RoomProperties startRoom = startRoomGO.AddComponent<RoomProperties>();

        startRoom.specialType = "start";
        startRoom.layoutPosition = 0;
        floorLayout[0] = startRoom;
        startRoom.exitList = new bool[4];
        startRoom.prevExit = -1; // startRoom doesn't have a prev exit... because it's the start

        for (int i = 0; i < startRoom.exitList.Length; i++)
        {
            startRoom.exitList[i] = false;
        }

        // adding the other rooms to the layout

        RoomProperties prevRoom = startRoom;

        chooseRandomExits(1, startRoom);

        for (int i = 1; i < floorLayout.Length; i++)
        {
            prevRoom = createAndAddRoomToLayout(prevRoom);
        }

        // prevRoom is now the bossRoom (last room)
        // removing exit from bossRoom

        prevRoom.exitList[prevRoom.nextExit] = false;
        prevRoom.nextExit = -1;
        prevRoom.specialType = "boss";

        addSpecialTypesToAll();
        AssignGraphicsPrefabPaths();
        InstantiateAllRooms();
        populateAllSpawnLists();

        gameManager.currentRoom = floorLayout[0];
        showRoom(0);
        enableAndDisableExitTileMaps();
    }

    RoomProperties createAndAddRoomToLayout(RoomProperties prevRoom)
    {
        GameObject newRoomGO = new GameObject();
        RoomProperties newRoom = newRoomGO.AddComponent<RoomProperties>();
        newRoom.layoutPosition = prevRoom.layoutPosition + 1;
        newRoom.name = "Room " + newRoom.layoutPosition;
        floorLayout[newRoom.layoutPosition] = newRoom;

        newRoom.exitList = new bool[4];
        for (int i = 0; i < newRoom.exitList.Length; i++)
        {
            newRoom.exitList[i] = false;
        }

        newRoom.prevExit = calculateNextRoomEntrance(prevRoom.nextExit);
        newRoom.exitList[newRoom.prevExit] = true;

        chooseRandomExits(1, newRoom);

        return newRoom;
    }

    public void showRoom(int indexOfRoom) // it also hides and shows the enemies
    {
        RoomProperties roomToShow = floorLayout[indexOfRoom];
        roomToShow.prefab.SetActive(true);

        for (int j = 0; j < roomToShow.spawnList.Count; j++)
        {
            GameObject spawn = (GameObject)roomToShow.spawnList[j];
            if (spawn != null) spawn.SetActive(true);
        }

        for (int i = 0; i < floorSize; i++)
        {
            if (i != indexOfRoom)
            {
                floorLayout[i].prefab.SetActive(false);

                for (int j = 0; j < floorLayout[i].spawnList.Count; j++)
                {
                    GameObject spawn = (GameObject)floorLayout[i].spawnList[j];
                    if (spawn != null) spawn.SetActive(false);
                }
            }
        }

    }

    private void addSpecialTypesToAll()
    {
        bool shopCreated = false;
        bool minibossCreated = false;
        bool sanctuaryCreated = false;
        bool chestCreated = false;
        int rand;

        RoomProperties currentRoom;
        for (int i = 0; i < floorSize; i++)
        {
            currentRoom = floorLayout[i];
            if (currentRoom.specialType == null)
            {
                rand = UnityEngine.Random.Range(0, 30);
                if ((rand == 3 || rand == 4) && (shopCreated == false))
                {
                    currentRoom.specialType = "shop";
                    shopCreated = true;
                }
                else if ((rand == 10) && (minibossCreated == false))
                {
                    currentRoom.specialType = "miniboss";
                    minibossCreated = true;
                }
                else if ((rand == 11 || rand == 19) && (sanctuaryCreated == false))
                {
                    currentRoom.specialType = "sanctuary";
                    sanctuaryCreated = true;
                }
                else if ((rand == 6 || rand == 7) && (chestCreated == false))
                {
                    currentRoom.specialType = "chest";
                    Debug.Log("Chest Room");
                }
                else currentRoom.specialType = "";
            }
        }
    }

    private void AssignGraphicsPrefabPaths()
    {

        RoomProperties currentRoom;

        for (int i = 0; i < floorSize; i++)
        {
            currentRoom = floorLayout[i];
            string prefabPath = "Prefab/Rooms/Room";

            if (currentRoom.exitList[0]) prefabPath += "North";
            if (currentRoom.exitList[2]) prefabPath += "South";
            if (currentRoom.exitList[1]) prefabPath += "East";
            if (currentRoom.exitList[3]) prefabPath += "West";

            if (currentRoom.specialType == "boss") prefabPath += "Boss";
            if (currentRoom.specialType == "shop") prefabPath += "Shop";
            if (currentRoom.specialType == "miniboss") prefabPath += "Miniboss";
            if (currentRoom.specialType == "chest") prefabPath += "Chest";
            if (currentRoom.specialType == "sanctuary") prefabPath += "Sanc";

            currentRoom.prefabPath = prefabPath;
        }
    }

    private void InstantiateAllRooms()
    {
        RoomProperties currentRoom;

        for (int i = 0; i < floorSize; i++)
        {
            currentRoom = floorLayout[i];
            currentRoom.prefab =  (GameObject)Instantiate(Resources.Load(currentRoom.prefabPath), new Vector3(0, 0, 0), Quaternion.identity, currentRoom.gameObject.transform);
        }
    }


    public int calculateNextRoomEntrance(int prevRoomExit) //-1 means you did it wrong
    {
        switch (prevRoomExit)
        {
            case 0: return 2;
            case 1: return 3;
            case 2: return 0;
            case 3: return 1;
            default: return -1;
        }
    }

    public void enableAndDisableExitTileMaps()
    {
        ExitHandler[] exits = GameObject.FindObjectsOfType<ExitHandler>();

        foreach (ExitHandler exit in exits)
        {
            if ((gameManager.currentRoom.nextExit == exit.exitDirection) || (gameManager.currentRoom.prevExit == exit.exitDirection))
                exit.transform.Find("Tilemap").gameObject.SetActive(true);
            else exit.transform.Find("Tilemap").gameObject.SetActive(false);
        }

    }

    private void populateAllSpawnLists()
    {
        RoomProperties currentRoom;
        for (int i = 0; i < floorSize; i++)
        {
            currentRoom = floorLayout[i];
            randomlyPopulateSpawnList(currentRoom);

            //TODO: Add spawnlist support for special rooms

        }
    }

    void randomlyPopulateSpawnList(RoomProperties room) //populate a room with enemies and other stuff
    {
        room.spawnList = getRandomEnemyList(room);
        List<GameObject> objectList = getRandomObjectList(room);

        for (int i = 0; i < objectList.Count; i++)
        {
            room.spawnList.Add(objectList[i]);
        }

    }

    List<GameObject> getRandomEnemyList(RoomProperties room)
    {
        int enemyAmount = UnityEngine.Random.Range(1, maxNumberOfEnemiesPerRoom + 1);

        List<GameObject> enemyList = new List<GameObject>();

        if (room.specialType == "") { 

            for (int i = 0; i < enemyAmount; i++)
            {
                string enemyType = allowedEnemies[UnityEngine.Random.Range(0, allowedEnemies.Length)];

                string selectedEnemyPath = "Prefab/Enemies/" + enemyType;
                Vector2 selectedEnemySpawnCoords = new Vector2(UnityEngine.Random.Range(-5.5f, 6.5f), UnityEngine.Random.Range(-5.25f, 3));
                GameObject newEnemy = (GameObject)Instantiate(Resources.Load(selectedEnemyPath), selectedEnemySpawnCoords, Quaternion.identity, room.gameObject.transform);

                Spawnable newEnemySpawnable = newEnemy.GetComponent<Spawnable>();
                newEnemySpawnable.spawnRoom = room;
                newEnemySpawnable.spawnCoords = selectedEnemySpawnCoords;

                EnemyBehavior newEnemyBehavior = newEnemy.GetComponent<EnemyBehavior>();
                if (!GameManager.enemyWinText.TryGetValue(enemyType, out newEnemyBehavior.duelWinText)) newEnemyBehavior.duelWinText = "You may choose 1 card from your opponent's deck to add to your deck.";
                if (!GameManager.enemyLossText.TryGetValue(enemyType, out newEnemyBehavior.duelLossText)) newEnemyBehavior.duelLossText = "You may replace up to three cards from your deck with randomly selected cards of the same type. When finished, press End to go back to the main menu and begin a new run.";
                if (!GameManager.enemyDuelConditions.TryGetValue(enemyType, out newEnemyBehavior.duelSpecialCondition)) newEnemyBehavior.duelSpecialCondition = "Normal duel - No specific conditions";
                if (!GameManager.enemyHeaders.TryGetValue(enemyType, out newEnemyBehavior.duelHeaderText)) newEnemyBehavior.duelHeaderText = "Error - Missing Header Text";
                if (!GameManager.enemyMoneyRewards.TryGetValue(enemyType, out newEnemyBehavior.rewardMoney)) newEnemyBehavior.rewardMoney = UnityEngine.Random.Range(100, 300);
                if (!GameManager.enemyXPRewards.TryGetValue(enemyType, out newEnemyBehavior.rewardXP)) newEnemyBehavior.rewardXP = UnityEngine.Random.Range(5, 10);

                newEnemyBehavior.duelWinText = newEnemyBehavior.duelWinText + "\nYou also win " + newEnemyBehavior.rewardMoney + " gold!";

                enemyList.Add(newEnemy);
            }
        } else if (room.specialType == "miniboss")
        {
            string[] possibleBosses = getBossArrayForFloorNum(floorNumber, "miniboss");

            string bossName = possibleBosses[UnityEngine.Random.Range(0, possibleBosses.Length)];
            string selectedBossPath = "Prefab/Minibosses/" + bossName;
            Debug.Log(selectedBossPath);
            Vector2 selectedEnemySpawnCoords = new Vector2(0.5f, -1.5f);
            GameObject newEnemy = (GameObject)Instantiate(Resources.Load(selectedBossPath), selectedEnemySpawnCoords, Quaternion.identity, room.gameObject.transform);

            Spawnable newEnemySpawnable = newEnemy.GetComponent<Spawnable>();
            newEnemySpawnable.spawnRoom = room;
            newEnemySpawnable.spawnCoords = selectedEnemySpawnCoords;

            EnemyBehavior newEnemyBehavior = newEnemy.GetComponent<EnemyBehavior>();
            if (!GameManager.enemyWinText.TryGetValue(bossName, out newEnemyBehavior.duelWinText)) newEnemyBehavior.duelWinText = "You did it! Select a card type (if it's a monster, choose the level as well)- you will receive three random cards of that type. Add any or all to your deck.";
            if (!GameManager.enemyLossText.TryGetValue(bossName, out newEnemyBehavior.duelLossText)) newEnemyBehavior.duelLossText = "You may replace up to three cards from your deck with randomly selected cards of the same type. When finished, press End to go back to the main menu and begin a new run.";
            if (!GameManager.enemyDuelConditions.TryGetValue(bossName, out newEnemyBehavior.duelSpecialCondition)) newEnemyBehavior.duelSpecialCondition = "Error - Boss Condition Missing";
            if (!GameManager.enemyHeaders.TryGetValue(bossName, out newEnemyBehavior.duelHeaderText)) newEnemyBehavior.duelHeaderText = "Error - Missing Header Text";
            if (!GameManager.enemyMoneyRewards.TryGetValue(bossName, out newEnemyBehavior.rewardMoney)) newEnemyBehavior.rewardMoney = UnityEngine.Random.Range(500, 800);
            if (!GameManager.enemyXPRewards.TryGetValue(bossName, out newEnemyBehavior.rewardXP)) newEnemyBehavior.rewardXP = UnityEngine.Random.Range(10, 20);

            newEnemyBehavior.duelWinText = newEnemyBehavior.duelWinText + "\nYou also win " + newEnemyBehavior.rewardMoney + " gold!";

            enemyList.Add(newEnemy);
        } else if (room.specialType == "boss")
        {

            string[] possibleBosses = getBossArrayForFloorNum(floorNumber, "boss");

            string bossName = possibleBosses[UnityEngine.Random.Range(0, possibleBosses.Length)];
            string selectedBossPath = "Prefab/Bosses/" + bossName;
            Debug.Log(selectedBossPath);
            Vector2 selectedEnemySpawnCoords = new Vector2(0.5f,-1.5f);
            GameObject newEnemy = (GameObject)Instantiate(Resources.Load(selectedBossPath), selectedEnemySpawnCoords, Quaternion.identity, room.gameObject.transform);

            Spawnable newEnemySpawnable = newEnemy.GetComponent<Spawnable>();
            newEnemySpawnable.spawnRoom = room;
            newEnemySpawnable.spawnCoords = selectedEnemySpawnCoords;

            EnemyBehavior newEnemyBehavior = newEnemy.GetComponent<EnemyBehavior>();
            if (!GameManager.enemyWinText.TryGetValue(bossName, out newEnemyBehavior.duelWinText)) newEnemyBehavior.duelWinText = "You did it! Select a card type (if it's a monster, choose the level as well)- you will receive five random cards of that type. Add any or all to your deck.";
            if (!GameManager.enemyLossText.TryGetValue(bossName, out newEnemyBehavior.duelLossText)) newEnemyBehavior.duelLossText = "You may replace up to three cards from your deck with randomly selected cards of the same type. When finished, press End to go back to the main menu and begin a new run.";
            if (!GameManager.enemyDuelConditions.TryGetValue(bossName, out newEnemyBehavior.duelSpecialCondition)) newEnemyBehavior.duelSpecialCondition = "Error - Boss Condition Missing";
            if (!GameManager.enemyHeaders.TryGetValue(bossName, out newEnemyBehavior.duelHeaderText)) newEnemyBehavior.duelHeaderText = "Error - Missing Header Text";
            if (!GameManager.enemyMoneyRewards.TryGetValue(bossName, out newEnemyBehavior.rewardMoney)) newEnemyBehavior.rewardMoney = UnityEngine.Random.Range(500, 800);
            if (!GameManager.enemyXPRewards.TryGetValue(bossName, out newEnemyBehavior.rewardXP)) newEnemyBehavior.rewardXP = UnityEngine.Random.Range(25, 30);

            newEnemyBehavior.duelWinText = newEnemyBehavior.duelWinText + "\nYou also win " + newEnemyBehavior.rewardMoney + " gold!";

            enemyList.Add(newEnemy);
        }

        

        return enemyList;
    }

    private string[] getBossArrayForFloorNum(int floorNumber, string bossOrMiniboss) 
    {
        if (bossOrMiniboss == "boss")
        {
            if (floorNumber == 1) return GameManager.floorOneBosses;
            if (floorNumber == 2) return GameManager.floorTwoBosses;
            if (floorNumber == 3) return GameManager.floorThreeBosses;
            if (floorNumber == 4) return GameManager.floorFourBosses;
        }
        else if (bossOrMiniboss == "miniboss")
        {
            if (floorNumber == 1) return GameManager.floorOneMiniBosses;
            if (floorNumber == 2) return GameManager.floorTwoMiniBosses;
            if (floorNumber == 3) return GameManager.floorThreeMiniBosses;
            if (floorNumber == 4) return GameManager.floorFourMiniBosses;
        }
        
        return GameManager.floorOneBosses; // default value, shouldn't be reached unless bosses for a particular floor weren't implemented yet

    }

    List<GameObject> getRandomObjectList(RoomProperties room)
    {
        int objectAmount = UnityEngine.Random.Range(maxNumberOfItemsPerRoom/2, maxNumberOfItemsPerRoom + 1);

        List<GameObject> objectList = new List<GameObject>();

        if (room.specialType == "")
        {

            for (int i = 0; i < objectAmount; i++)
            {
                string objectType = allowedObjects[UnityEngine.Random.Range(0, allowedObjects.Length)];

                string selectedObjectPath = "Prefab/Objects/" + objectType;
                Vector2 selectedObjectSpawnCoords = new Vector2(UnityEngine.Random.Range(-5.5f, 6.5f), UnityEngine.Random.Range(-5.25f, 3));
                GameObject newObject = (GameObject)Instantiate(Resources.Load(selectedObjectPath), selectedObjectSpawnCoords, Quaternion.identity, room.gameObject.transform);

                Spawnable newObjectSpawnable = newObject.GetComponent<Spawnable>();
                newObjectSpawnable.spawnRoom = room;
                newObjectSpawnable.spawnCoords = selectedObjectSpawnCoords;
                
                objectList.Add(newObject);
            }
        }

        return objectList;
    }


    void chooseRandomExits(int numOfExits, RoomProperties room)
    {
        for (int i = 0; i < numOfExits ; i++)
        {
            ArrayList tempNumArray = new ArrayList { 0, 1, 2, 3 };
            int tempNum = UnityEngine.Random.Range(0, tempNumArray.Count - 1);
            

            while (room.exitList[tempNum])
            {
                tempNumArray.Remove(tempNum);
                tempNum = UnityEngine.Random.Range(0, tempNumArray.Count - 1);
            }

            room.nextExit = tempNum;
            room.exitList[tempNum] = true;
                      
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}


