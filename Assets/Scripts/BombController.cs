using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject explosion;
    public GameObject[] test;
    public LayerMask whatIsDestucable;
    public LayerMask whatIsDamageable;
    public int damageAmount;
    public float timeToExplode=0.5f;
    public float blastRange;

    void Update()
    {
        timeToExplode-=Time.deltaTime;
        if (timeToExplode<=0)
        {
            if (explosion) 
            {
                Instantiate(explosion,transform.position,transform.rotation);
            }
            Destroy(gameObject);
            Collider2D[] objsToDamage=
                Physics2D.OverlapCircleAll(transform.position,blastRange,whatIsDestucable);
            foreach (var col in objsToDamage)
            {
                Destroy(col.gameObject);
            }
            objsToDamage =
                Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsDamageable);
            foreach (var col in objsToDamage)
            {
                EnemyHealthController enemyHealth=col.GetComponent<EnemyHealthController>();
                if (enemyHealth != null) 
                {
                    enemyHealth.DamageEnemy(damageAmount);
                }
            }
            AudioManager.instance.PlaySFXWithRandomPitch(4);
        }
    }
}
