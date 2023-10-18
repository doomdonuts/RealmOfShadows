using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollideDamage : MonoBehaviour
{
    PlayerHealth player;
    public int damageAmount;
    AudioSource audioSource;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerHealth>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(clip);
            player.currentHealth -= damageAmount;
        }
    }

}
