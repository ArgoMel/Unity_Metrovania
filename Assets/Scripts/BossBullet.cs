using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public GameObject impactEffect;
    public int damageAmount;
    public float moveSpeed;

    private Rigidbody2D theRB;

    private void Awake()
    {
        theRB = GetComponent<Rigidbody2D>();    
    }

    private void Start()
    {
        Vector3 dir = transform.position - PlayerHealthController.instance.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Update()
    {
        theRB.velocity = -transform.right * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            PlayerHealthController.instance.DamagePlayer(damageAmount);
        }
        if (impactEffect)
        {
            Instantiate(impactEffect,transform.position,transform.rotation);
        }
        Destroy(gameObject);
    }
}
