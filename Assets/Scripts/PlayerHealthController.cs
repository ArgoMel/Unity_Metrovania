using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int curHealth;
    public int maxHealth;

    public SpriteRenderer[] playerSprites;
    public float invincibilityLength;
    public float flashLength;
    private float flashCounter;
    private float invincCounter;

    private void Awake()
    {
        if (!instance) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (invincCounter > 0)
        {
            invincCounter-=Time.deltaTime;
            flashCounter-=Time.deltaTime;
            if (flashCounter<=0)
            {
                foreach (var sr in playerSprites)
                {
                    sr.enabled = !sr.enabled;
                }
                flashCounter = flashLength;
            }
            if(invincCounter <= 0) 
            {
                foreach (var sr in playerSprites)
                {
                    sr.enabled = true;
                }
                flashCounter = 0;
            }
        }
    }

    public void DamagePlayer(int damageAmount) 
    {
        if (invincCounter>0)
        {
            return;
        }
        curHealth-=damageAmount;
        if (curHealth<=0)
        {
            curHealth=0;
            RespawnController.instance.Respawn();
            AudioManager.instance.PlaySFX(8);
        }
        else 
        {
            invincCounter=invincibilityLength;
            AudioManager.instance.PlaySFXWithRandomPitch(11);
        }
        UIController.instance.UpdateHealth(curHealth, maxHealth);
    }

    public void ModifyHealth(int healAmount) 
    {
        curHealth += healAmount;
        if (curHealth>maxHealth)
        {
            curHealth = maxHealth;  
        }
        UIController.instance.UpdateHealth(curHealth, maxHealth);
    }

    public void ResetHealth() 
    {
        ModifyHealth(maxHealth);
    }
}
