using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DuelTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    private EnemyBehavior enemy;
    private GameManager gameManager;
    private AudioSource audioSource;
    public AudioClip useSpecialSound;
    public AudioClip errorSound;

    private bool isDuelWon = false;
    private bool isSpecialAvailable = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = gameObject.GetComponent<AudioSource>();

        gameManager.winButton.onClick.AddListener(duelWon);
        gameManager.loseButton.onClick.AddListener(duelLost);
        gameManager.endButton.onClick.AddListener(duelEnd);
        gameManager.specialDeckButton.onClick.AddListener(duelSpecial);

    }

    private void duelSpecial()
    {
        if (isSpecialAvailable)
        {
            if (GameData.playerXP >= 100)
            {
                audioSource.PlayOneShot(useSpecialSound);
                GameData.playerXP = 0;
            } else
            {
                audioSource.PlayOneShot(errorSound);
            }
        } else
        {
            if (GameData.playerXP < 100)
            {
                audioSource.PlayOneShot(useSpecialSound);
                GameData.playerXP = (GameData.playerXP + 25 > 100) ? 100 : GameData.playerXP + 25;
                GameData.totalSacrificed += 1;
            } else
            {
                audioSource.PlayOneShot(errorSound);
            }
        }

    }

    private void duelEnd()
    {
        gameManager.winLoseButtons.SetActive(true);
        gameManager.afterDuel.SetActive(false);
        gameManager.duelObjects.SetActive(false);
        GameManager.isPaused = false;

        if (isDuelWon)
        {
            gameManager.currentRoom.spawnList.Remove(enemy.gameObject);
            Destroy(enemy.gameObject);
            GameSaveHandler.SaveGame();
        } else if (!isDuelWon) {
            GameSaveHandler.SaveGame();
            SceneManager.LoadScene(0);
        }

        
    }

    private void duelLost()
    {
        gameManager.winLoseButtons.SetActive(false);
        gameManager.header.text = "You Lost :(";
        gameManager.player.currentHealth = 0;
        gameManager.specialOrEndText.text = enemy.duelLossText;
        gameManager.afterDuel.SetActive(true);
        isDuelWon = false;
        GameData.totalDeaths += 1;
    }

    private void duelWon()
    {
        gameManager.winLoseButtons.SetActive(false);
        gameManager.header.text = "You win!";
        gameManager.specialOrEndText.text = enemy.duelWinText;
        gameManager.afterDuel.SetActive(true);
        isDuelWon = true;

        GameData.playerMoney += enemy.rewardMoney;
        if (GameData.gameState >= 3) GameData.playerXP = (GameData.playerXP + enemy.rewardXP >= 100) ? 100 : GameData.playerXP + enemy.rewardXP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerDuel(EnemyBehavior triggerer)
    {
        GameManager.isPaused = true;
        enemy = triggerer;
        //gameManager.GetComponent<AudioSource>();
        gameManager.header.text = enemy.duelHeaderText;
        gameManager.specialOrEndText.text = enemy.duelSpecialCondition;
        if (GameData.playerXP >= 100)
        {
            isSpecialAvailable = true;
            gameManager.specialDeckButtonText.text = "Use Special";

        } else
        {
            isSpecialAvailable = false;
            gameManager.specialDeckButtonText.text = "Relinquish";
        }


        gameManager.duelObjects.SetActive(true);
        
    }
}
