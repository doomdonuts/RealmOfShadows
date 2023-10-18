using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth;
    public int currentHealth;
    public int healthRegenPerSecond;
    public bool allowOverheal;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        //InvokeRepeating("processHealthRegen", 0.0f, 2.0f);
        

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void instantHealthAdjustment (int amountOfHealthToGive)
    {
        if (amountOfHealthToGive > 0)
        {
            if (currentHealth < startingHealth)
            {
                if ((currentHealth + amountOfHealthToGive > startingHealth) && (!allowOverheal)) currentHealth = startingHealth;
                else currentHealth += amountOfHealthToGive;
            }
        } else if (amountOfHealthToGive < 0)
        {
            if (currentHealth > 0)
            {
                if (currentHealth + amountOfHealthToGive < 0) currentHealth = 0;
                else currentHealth += amountOfHealthToGive;
            }
        }

    }

    void processHealthRegen()
    {
        if (!GameManager.isPaused)
        {
            if (healthRegenPerSecond > 0)
            {
                if (currentHealth < startingHealth)
                {
                    if (currentHealth + healthRegenPerSecond > startingHealth) currentHealth = startingHealth;
                    else currentHealth += healthRegenPerSecond;
                }
            }
            else if (healthRegenPerSecond < 0)
            {
                if (currentHealth > 0)
                {
                    if (currentHealth + healthRegenPerSecond < 0) currentHealth = 0;
                    else currentHealth += healthRegenPerSecond;
                }
            }
        }
    }
}
