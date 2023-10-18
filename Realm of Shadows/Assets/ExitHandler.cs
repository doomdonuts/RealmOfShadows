using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExitHandler : MonoBehaviour
{
    public DungeonGenerator dungeonManager;
    public GameManager gameManager;
    public int exitDirection;
    public TextMeshProUGUI instructions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2DChild(Collider2D other)
    {
        if ((instructions != null) && instructions.IsActive()) instructions.gameObject.SetActive(false);

        int whereToEnter = -1;
        bool changeAudio = false;

        if (this.exitDirection == gameManager.currentRoom.nextExit){ // going forwards through dungeon
            whereToEnter = dungeonManager.calculateNextRoomEntrance(gameManager.currentRoom.nextExit);
            if (gameManager.currentRoom.specialType != dungeonManager.floorLayout[gameManager.currentRoom.layoutPosition + 1].specialType)
            {
                if (!(gameManager.currentRoom.specialType == "start" && dungeonManager.floorLayout[gameManager.currentRoom.layoutPosition + 1].specialType == ""))
                    changeAudio = true;
            }
            gameManager.currentRoom = dungeonManager.floorLayout[gameManager.currentRoom.layoutPosition + 1];


        } else if (this.exitDirection == gameManager.currentRoom.prevExit) // backwards through dungeon
        {
            whereToEnter = dungeonManager.calculateNextRoomEntrance(gameManager.currentRoom.prevExit);
            if (gameManager.currentRoom.specialType != dungeonManager.floorLayout[gameManager.currentRoom.layoutPosition - 1].specialType)
            {
                if (!(gameManager.currentRoom.specialType == "" && dungeonManager.floorLayout[gameManager.currentRoom.layoutPosition - 1].specialType == "start"))
                    changeAudio = true;
            }
            gameManager.currentRoom = dungeonManager.floorLayout[gameManager.currentRoom.layoutPosition - 1];


        }
        if (gameManager.currentRoom.layoutPosition == 0)
        {
            gameManager.floorLabel.text = "Floor " + gameManager.currentFloorNum + "\nStart";
        } else
        {
            gameManager.floorLabel.text = "Floor " + gameManager.currentFloorNum + "\nRoom " + gameManager.currentRoom.layoutPosition;
        }
        dungeonManager.showRoom(gameManager.currentRoom.layoutPosition);
        GameObject.FindGameObjectWithTag("Player").transform.position = findCoordsForExit(whereToEnter);
        dungeonManager.enableAndDisableExitTileMaps();

        
        if (changeAudio) { 
            if (gameManager.currentRoom.specialType == "boss") gameManager.audioSource.clip = gameManager.bossMusic;
            else if (gameManager.currentRoom.specialType == "shop") gameManager.audioSource.clip = gameManager.shopMusic;
            else if (gameManager.currentRoom.specialType == "sanctuary") gameManager.audioSource.clip = gameManager.sanctuaryMusic;
            else if (gameManager.currentRoom.specialType == "miniboss") gameManager.audioSource.clip = gameManager.bossMusic;
            else if (gameManager.currentRoom.specialType == "chest") gameManager.audioSource.clip = gameManager.chestMusic;
            else gameManager.audioSource.clip = gameManager.mainFloorMusic;

            gameManager.audioSource.Play();
        }

    }

    private Vector2 findCoordsForExit(int exitNum)
    {
        switch (exitNum)
        {
            case 0: return new Vector2(0.5f, 6.0f); ;
            case 1: return new Vector2(10.0f, -1.5f); ;
            case 2: return new Vector2(0.5f, -11.0f); ;
            case 3: return new Vector2(-9.0f, -1.5f); ;
            default: return new Vector2(0.0f, 0.0f); ;
        }
    }



}
