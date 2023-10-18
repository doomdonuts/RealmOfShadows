using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTrigger : MonoBehaviour
{
    private TowardsEnemyWhenAlert parentBehavior;
    // Start is called before the first frame update
    void Start()
    {
        parentBehavior = this.GetComponentInParent<TowardsEnemyWhenAlert>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") parentBehavior.alerted = true;
    }
}
