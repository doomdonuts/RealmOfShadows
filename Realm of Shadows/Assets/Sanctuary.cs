using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanctuary : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip restoreHealth;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {


        if (other.gameObject.tag == "Player")
        {
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
            if (player.currentHealth < player.startingHealth)
            {
                audioSource.PlayOneShot(restoreHealth);
                player.currentHealth = player.startingHealth;
                GameSaveHandler.SaveGame();
            }
        }

    }
}
