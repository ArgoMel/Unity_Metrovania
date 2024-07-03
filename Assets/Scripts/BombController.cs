using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject explosion;
    public GameObject[] test;
    public LayerMask whatIsDestucable;
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
        }
    }
}
