using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public GameObject deathEffect;
    public int totalHealth = 3;

    public void DamageEnemy(int damageAmount) 
    {
        totalHealth-=damageAmount;
        if (totalHealth<=0)
        {
            if (deathEffect)
            {
                Instantiate(deathEffect,transform.position,transform.rotation);
            }
            AudioManager.instance.PlaySFX(4);
            Destroy(gameObject);
        }
    }
}
