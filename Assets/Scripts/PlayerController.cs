using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;

    public float moveSpeed;
    public float jumpForce;

    void Start()
    {
        
    }

    void Update()
    {
        //theRB.velocity = new Vector2(x, theRB.velocity.y);
    }
}
