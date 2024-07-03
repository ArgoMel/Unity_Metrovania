using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputRender input;
    private Rigidbody2D theRB;
    private PlayerAbilityTracker abilitiy;
    private Vector2 moveDir;
    private bool isOnGround=false;

    public Transform groundPoint;
    public Animator anim;
    public LayerMask whatIsGround;
    public float moveSpeed;
    public float jumpForce;
    public bool canDoubleJump;

    public BulletController shotToFire;
    public Transform shotPoint;

    private float dashCounter;
    public float dashSpeed;
    public float dashTime;

    private float afterImgCounter;
    private float dashRechargeCounter;
    public SpriteRenderer theSR;
    public SpriteRenderer afterImg;
    public Color afterImgColor;
    public float afterImgLifetime;
    public float timeBetweenAfterImgs;
    public float waitAfterDashing=0.25f;

    private float ballCounter;
    public GameObject standing;
    public GameObject ball;
    public GameObject bomb;
    public Animator ballAnim;
    public Transform bombPoint;
    public float waitToBall;

    private void Awake()
    {
        theRB = GetComponent<Rigidbody2D>();
        abilitiy=GetComponent<PlayerAbilityTracker>();
    }

    void Start()
    {
        input.MoveEvent += OnMove;
        input.MoveCanceledEvent += OnMoveCanceled;
        input.JumpEvent += OnJump;
        input.FireEvent += OnFire;
        input.DashEvent += OnDash;
    }

    void Update()
    {
        dashRechargeCounter-=Time.deltaTime;

        if (abilitiy.canBecomeBall)
        {
            if (!ball.activeSelf &&
                moveDir.y < -0.9f)
            {
                ballCounter -= Time.deltaTime;
                if (ballCounter <= 0)
                {
                    ball.SetActive(true);
                    standing.SetActive(false);
                }
            }
            else if (ball.activeSelf &&
                moveDir.y > 0.9f)
            {
                ballCounter -= Time.deltaTime;
                if (ballCounter <= 0)
                {
                    ball.SetActive(false);
                    standing.SetActive(true);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            theRB.velocity = new Vector2(dashSpeed * transform.localScale.x, theRB.velocity.y);
            afterImgCounter-=Time.deltaTime;
            if (afterImgCounter>=0)
            {
                ShowAfterImage();
            }
            dashRechargeCounter=waitAfterDashing;
        }
        else
        {
            theRB.velocity = new Vector2(moveDir.x * moveSpeed, theRB.velocity.y);
            if (theRB.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (theRB.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
            }
        }
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, whatIsGround);
    }

    private void LateUpdate()
    {
        UpdateAnim();
    }

    private void OnMove(Vector2 dir)
    {
        moveDir = dir;
    }

    private void OnMoveCanceled(Vector2 vector)
    {
        ballCounter = waitToBall;
    }

    private void OnJump()
    {
        if (isOnGround||
            (canDoubleJump&&abilitiy.canDoubleJump))
        {
            canDoubleJump = isOnGround;
            if (!canDoubleJump)
            {
                anim.SetTrigger("doubleJump");
            }
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }
    }

    private void OnFire()
    {
        if (standing.activeSelf)
        {
            BulletController bullet = Instantiate(shotToFire, shotPoint.position, shotPoint.rotation);
            bullet.moveDir = new Vector2(transform.localScale.x, 0f);
            anim.SetTrigger("shotFired");
        }
        else if (ball.activeSelf&&abilitiy.canDropBomb) 
        {
            Instantiate(bomb,bombPoint.position, bombPoint.rotation);
        }
    }

    private void OnDash()
    {
        if (dashRechargeCounter>0||
            !standing.activeSelf||
            !abilitiy.canDash)
        {
            return;
        }
        dashCounter = dashTime;
        ShowAfterImage();
    }

    private void UpdateAnim()
    {
        if (standing.activeSelf)
        {
            anim.SetBool("isOnGround", isOnGround);
            anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        }
        if (ball.activeSelf)
        {
            ballAnim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        }
    }

    public void ShowAfterImage() 
    {
        SpriteRenderer image= Instantiate(afterImg,transform.position,transform.rotation);
        image.sprite = theSR.sprite;
        image.transform.localScale=transform.localScale;
        image.color=afterImgColor;
        Destroy(image.gameObject, afterImgLifetime);
        afterImgCounter = timeBetweenAfterImgs;
    }
}
