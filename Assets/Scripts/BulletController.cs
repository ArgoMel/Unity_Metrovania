using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public GameObject impactEffect;
    public Vector2 moveDir;
    public float bulletSpeed;

    void Update()
    {
        theRB.velocity=moveDir*bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
