using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public GameObject impactEffect;
    public Vector2 moveDir;
    public int damageAmount = 1;
    public float bulletSpeed;

    void Update()
    {
        theRB.velocity=moveDir*bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Enemy")
        {
            collision.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }
        if (collision.tag == "Boss")
        {
            BossHealthController.instance.TakeDamage(damageAmount);
        }
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
        AudioManager.instance.PlaySFXWithRandomPitch(3);
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
