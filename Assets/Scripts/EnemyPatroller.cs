using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    private Rigidbody2D theRB;
    private int curPoint=0;
    private float waitCounter;

    public Transform[] patrolPoints;
    public Animator anim;
    public float moveSpeed;
    public float waitAtPoint;
    public float jumpForce;

    private void Awake()
    {
        theRB=GetComponent<Rigidbody2D>();  
    }

    void Start()
    {
        waitCounter=waitAtPoint;
        foreach (var point in patrolPoints)
        {
            point.SetParent(null);
        }
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x - patrolPoints[curPoint].position.x)>0.2f)
        {
            if (transform.position.x < patrolPoints[curPoint].position.x)
            {
                theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
                transform.localScale = Vector3.one;
            }

            if (transform.position.y < patrolPoints[curPoint].position.y-0.5f&&
                theRB.velocity.y<0.1f)
            {
                theRB.velocity = new Vector2(theRB.velocity.x,jumpForce);
            }
        }
        else 
        {
            theRB.velocity = new Vector2(0f,theRB.velocity.y);
            waitCounter-=Time.deltaTime;
            if (waitCounter<=0) 
            {
                waitCounter = waitAtPoint;
                curPoint=(curPoint+1)% patrolPoints.Length;
            }
        }

        anim.SetFloat("speed",Mathf.Abs(theRB.velocity.x));
    }
}
