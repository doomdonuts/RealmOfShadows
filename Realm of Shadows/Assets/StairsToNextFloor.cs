using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StairsToNextFloor : MonoBehaviour
{
    GameManager gameManager;
    AudioSource audioSource;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();   
        audioSource = gameManager.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int floorToGoTo = gameManager.currentFloorNum + 1;
        
        audioSource.PlayOneShot(clip);
        StartCoroutine(FadeAudioSource.StartFade(audioSource, 1.0f, 0.0f));
        if (GameData.unlockedFloors < floorToGoTo)
        {
            GameData.unlockedFloors = floorToGoTo;
            Debug.Log("Unlocked floor " + GameData.unlockedFloors);
        }


        if ((GameData.gameState < 2) && (gameManager.currentFloorNum == 1)) floorToGoTo = 6; //Heart of the Cards cutscene
        if ((GameData.gameState < 3) && (gameManager.currentFloorNum == 2)) floorToGoTo = 7; //Meeting Eyes cutscene
        if ((GameData.gameState < 4) && (gameManager.currentFloorNum == 3)) floorToGoTo = 8; //Eyes and Chazz speak cutscene
        if ((GameData.gameState < 5) && (gameManager.currentFloorNum == 4)) floorToGoTo = 9; //Final Cutscene
        if ((GameData.gameState >= 5) && (gameManager.currentFloorNum == 4)) floorToGoTo = 10; //End Screen

        Debug.Log("Gamestate: " + GameData.gameState);
        Debug.Log("Current floor number: "+ gameManager.currentFloorNum);
        Debug.Log("Going to scene " + floorToGoTo);

        GameSaveHandler.SaveGame();
        SceneManager.LoadScene(floorToGoTo);
        
    }
}
