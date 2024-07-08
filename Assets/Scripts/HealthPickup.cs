using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public GameObject pickupEffect;
    public int healthAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            PlayerHealthController.instance.ModifyHealth(healthAmount);
            if (pickupEffect) 
            {
                Instantiate(pickupEffect,transform.position, Quaternion.identity);  
            }
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(5);
        }
    }
}
