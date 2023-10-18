using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileTriggerSlow : MonoBehaviour
{
    PlayerMovement playerMove;
    public float slowAmount;
    AudioSource audioSource;
    float normalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GameObject.FindObjectOfType<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        normalSpeed = playerMove.maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (slowAmount > 1)
            {
                playerMove.initialSpeed = playerMove.maxSpeed * slowAmount;
            }

            playerMove.maxSpeed = playerMove.maxSpeed * slowAmount;

            audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerMove.maxSpeed = normalSpeed;
            audioSource.Stop();
        }
    }
}
