using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Platformer2d controls;
    private bool isOnGround=false;

    public Rigidbody2D theRB;
    public Transform groundPoint;
    public Animator anim;
    public LayerMask whatIsGround;
    public float moveSpeed;
    public float jumpForce;

    private void Awake()
    {
        controls = new Platformer2d();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        controls.Player.Move.performed += ctx => OnMovePerformed(ctx.ReadValue<Vector2>());
        controls.Player.Jump.performed += ctx => Jump();
    }

    void Update()
    {
        if (theRB.velocity.x<0)
        {
            transform.localScale = new Vector3(-1f,1f,1f);
        }
        else if (theRB.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, whatIsGround);
        UpdateAnim();
    }

    private void OnMovePerformed(Vector2 dir)
    {
        theRB.velocity = new Vector2(dir.x * moveSpeed, theRB.velocity.y);
    }

    private void Jump()
    {
        if (isOnGround)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }
    }

    private void UpdateAnim()
    {
        anim.SetBool("isOnGround", isOnGround);
        anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
    }
}
