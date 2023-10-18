using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomProperties : MonoBehaviour
{
    public List<GameObject> spawnList = new List<GameObject>(); // list of all enemies and objects in the room
    public bool[] exitList; // true if there is an exit: index 0 is north, 1 is east, 2 is south, 3 is west 
    public string specialType; // possible options: start, shop, sanctuary, miniboss, chest, challenge, boss, none
    public int layoutPosition; // x and y position in the grid of rooms
    public string prefabPath; // Resources path of the graphics prefab that is made for this room
    public GameObject prefab;
    public int prevExit;
    public int nextExit;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
