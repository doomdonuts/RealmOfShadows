using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBehavior : MonoBehaviour {

    public AudioClip duelBGM;
    public string duelWinText = "";
    public string duelLossText = "";
    public string duelSpecialCondition = "";
    public string duelHeaderText = "";
    public int rewardMoney = 0;
    public int rewardXP = 0;

    private GameManager gameManager;
    
    private Animator animator;

    private DuelTrigger duelTrigger;

    public delegate void EnemyMovementDelegate();

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        animator = this.GetComponent<Animator>();

        duelTrigger = gameManager.GetComponent<DuelTrigger>();
    }

    // Update is called once per frame
    void Update()   
    {
        
    }

    public void ExecuteEnemyMovement(EnemyMovementDelegate movementMethod)
    {
        movementMethod();
    }
   

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (animator != null) animator.enabled = false;
            duelTrigger.TriggerDuel(this);
            
        }
    }

}


