using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileTriggerSlide : MonoBehaviour
{
    GameObject player;
    PlayerMovement playerMove;
    Rigidbody2D playerPhys;
    public float slowAmount;
    AudioSource audioSource;
    float normalSpeed;
    float normalFriction;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GameObject.FindObjectOfType<PlayerMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
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
