using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public GameObject destroyEffect;
    public int damageAmount = 1;
    public bool destroyOnDamage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DealDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            DealDamage();
        }
    }

    void DealDamage() 
    {
        PlayerHealthController.instance.DamagePlayer(damageAmount);
        if (destroyOnDamage)
        {
            if (destroyEffect)
            {
                Instantiate(destroyEffect, transform.position,transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}
