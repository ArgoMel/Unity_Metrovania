using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputRender input;
    private Vector2 moveDir;
    private bool isOnGround=false;

    public Rigidbody2D theRB;
    public Transform groundPoint;
    public Animator anim;
    public LayerMask whatIsGround;
    public float moveSpeed;
    public float jumpForce;

    public BulletController shotToFire;
    public Transform shotPoint;

    void Start()
    {
        input.MoveEvent += OnMove;
        input.JumpEvent += OnJump;
        input.FireEvent += OnFire;
        input.DashEvent += OnDash;
    }

    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, whatIsGround);
        theRB.velocity= new Vector2(moveDir.x * moveSpeed, theRB.velocity.y);
    }

    private void LateUpdate()
    {
        if (theRB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (theRB.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        UpdateAnim();
    }

    private void OnMove(Vector2 dir)
    {
        moveDir = dir;
    }

    private void OnJump()
    {
        if (isOnGround)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }
    }

    private void OnFire()
    {
        BulletController bullet=Instantiate(shotToFire,shotPoint.position,shotPoint.rotation);
        bullet.moveDir = new Vector2(transform.localScale.x,0f);
        anim.SetTrigger("shotFired");
    }

    private void OnDash()
    {
        
    }

    private void UpdateAnim()
    {
        anim.SetBool("isOnGround", isOnGround);
        anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
    }
}
