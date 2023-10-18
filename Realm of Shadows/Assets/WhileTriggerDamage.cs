using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileTriggerDamage : MonoBehaviour
{
    PlayerHealth playerHealth;
    PlayerMovement playerMove;
    public int damageAmount;
    AudioSource audioSource;
    bool isDamage = false;
    private float damageTickSeconds = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDamage && !GameManager.isPaused && damageTickSeconds <= 0)
        {
            playerHealth.currentHealth = (playerHealth.currentHealth - damageAmount) >= 0 ? (int)(playerHealth.currentHealth - damageAmount) : 0;
            damageTickSeconds = 1.0f;
            
        } else if (damageTickSeconds > 0)
        {
            damageTickSeconds -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") isDamage = true;
        audioSource.Play();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") isDamage = false;
        audioSource.Stop();
    }
}
