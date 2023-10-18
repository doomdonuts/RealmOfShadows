using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChazzDuelTrigger : MonoBehaviour
{
    public EnemyBehavior enemy;
    public DuelTrigger duelTrigger;
    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.GetComponentInParent<EnemyBehavior>();
        duelTrigger = GameObject.FindObjectOfType<DuelTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void callDuelMenu()
    {
        duelTrigger.TriggerDuel(enemy);
    }
}
