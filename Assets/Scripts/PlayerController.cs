using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Space]
    [Header("Component")]
    [SerializeField] private InputRender input;
    private Rigidbody2D theRB;
    private PlayerAbilityTracker abilitiy;
    private Collision coll;
    public SpriteRenderer theSR;
    public Animator anim;

    [Space]
    [Header("Stats")]
    public bool canMove;
    public float moveSpeed;

    private Vector2 moveDir;

    [Space]
    [Header("Wall")]
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;

    [Space]
    [Header("Jump")]
    public float jumpForce;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    private bool isJumping;
    private bool canDoubleJump;

    [Space]
    [Header("Bullet")]
    public BulletController shotToFire;
    public Transform shotPoint;

    [Space]
    [Header("Dash")]
    public float dashSpeed;
    private GhostTrail trail;
    private bool hasDashed;
    private bool isDashing;

    [Space]
    [Header("Ball")]
    private float ballCounter;
    private SpriteRenderer ballSR;

    [Space]
    [Header("Bomb")]
    public GameObject standing;
    public GameObject ball;
    public GameObject bomb;
    public Animator ballAnim;
    public Transform bombPoint;
    public float waitToBall;

    [Space]
    [Header("Particle")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;

    private void Awake()
    {
        theRB = GetComponent<Rigidbody2D>();
        abilitiy = GetComponent<PlayerAbilityTracker>();
        coll=GetComponent<Collision>();
        ballSR= ball.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        input.MoveEvent += OnMove;
        input.MoveCanceledEvent += OnMoveCanceled;
        input.JumpEvent += OnJump;
        input.JumpCanceledEvent += OnJumpCanceled;
        input.FireEvent += OnFire;
        input.DashEvent += OnDash;
        input.WallGrabEvent += OnWallGrab;
        input.WallGrabCanceledEvent += OnWallGrabCanceled;
        input.PauseEvent += OnPause;
        input.SetGamePlay();

        coll.GroundTouchEvent += OnGroundTouch;
        coll.WallOutEvent += OnWallGrabCanceled;

        trail = FindObjectOfType<GhostTrail>();
        canMove = true;

        Debug.Log("Player Start");
    }

    void Update()
    {
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
                    AudioManager.instance.PlaySFX(6);
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
                    AudioManager.instance.PlaySFX(10);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Walk();
        if (wallGrab && 
            !isDashing)
        {
            theRB.gravityScale = 0;
            if (moveDir.x > .2f || moveDir.x < -.2f)
            {
                theRB.velocity = new Vector2(theRB.velocity.x, 0);
            }
            float speedModifier = moveDir.y > 0 ? .5f : 1;
            theRB.velocity = new Vector2(theRB.velocity.x, moveDir.y * (moveSpeed * speedModifier));
        }
        else
        {
            theRB.gravityScale = 3;
        }

        WallSlide();

        if (theRB.velocity.y < 0)
        {
            theRB.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (theRB.velocity.y > 0 && !isJumping)
        {
            theRB.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        WallParticle(moveDir.y);
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
        if (!canMove)
        {
            return;
        }
        if (coll.onGround ||
            (canDoubleJump && abilitiy.canDoubleJump))
        {
            canDoubleJump = coll.onGround;
            if (!canDoubleJump)
            {
                anim.SetTrigger("doubleJump");
                AudioManager.instance.PlaySFXWithRandomPitch(9);
            }
            isJumping = true;
            Jump(Vector2.up, false);
        }
        if (coll.onWall && 
            !coll.onGround)
        {
            WallJump();
        }
    }

    private void OnJumpCanceled()
    {
        isJumping = false;
    }

    private void OnFire()
    {
        if (!canMove)
        {
            return;
        }
        if (standing.activeSelf)
        {
            BulletController bullet = Instantiate(shotToFire, shotPoint.position, shotPoint.rotation);
            bullet.moveDir = new Vector2(transform.localScale.x, 0f);
            anim.SetTrigger("shotFired");
            AudioManager.instance.PlaySFXWithRandomPitch(14);
        }
        else if (ball.activeSelf && abilitiy.canDropBomb)
        {
            Instantiate(bomb, bombPoint.position, bombPoint.rotation);
            AudioManager.instance.PlaySFXWithRandomPitch(13);
        }
    }

    private void OnDash()
    {
        if (!standing.activeSelf ||
            hasDashed||
            isDashing||
            !abilitiy.canDash||
            !canMove)
        {
            return;
        }
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
        hasDashed = true;
        theRB.velocity = Vector2.zero;
        theRB.velocity += moveDir * dashSpeed;
        StartCoroutine(DashWait());
        AudioManager.instance.PlaySFXWithRandomPitch(7);
    }

    private void OnWallGrab()
    {
        if (!coll.onWall ||
            !standing.activeSelf ||
            !canMove)
        {
            return;
        }
        if (transform.localScale.x != coll.wallSide)
        {
            Flip((int)(transform.localScale.x * -1));
        }
        wallGrab = true;
        wallSlide = false;
    }

    private void OnWallGrabCanceled()
    {
        wallGrab = false;
        if (!coll.onWall ||
            !canMove)
        {
            wallSlide = false;
        }
    }

    private void OnGroundTouch()
    {
        hasDashed = false;
        isDashing = false;
        jumpParticle.Play();
    }

    private void OnPause()
    {
        UIController.instance.TogglePause();
    }

    private void Walk() 
    {
        if (!canMove ||
           wallGrab)
        {
            return;
        }
        if (!wallJumped)
        {
            theRB.velocity = new Vector2(moveDir.x * moveSpeed, theRB.velocity.y);
        }
        else
        {
            theRB.velocity = Vector2.Lerp(
                theRB.velocity, (new Vector2(moveDir.x * moveSpeed, theRB.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
        if (theRB.velocity.x < 0)
        {
            Flip(-1);
        }
        else if (theRB.velocity.x > 0)
        {
            Flip(1);
        }
    }

    private void Jump(Vector2 dir, bool wall)
    {
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;
        theRB.velocity = new Vector2(theRB.velocity.x, 0);
        theRB.velocity += dir * jumpForce;
        particle.Play();
        AudioManager.instance.PlaySFX(12);
    }

    private void WallSlide()
    {
        wallSlide = coll.onWall && !coll.onGround&& standing.activeSelf;
        if (!wallSlide||
            moveDir.x==0||
            wallGrab)
        {
            return ;
        }
        if (coll.wallSide != transform.localScale.x)
        {
            Flip((int)(transform.localScale.x * -1));
        }
        if (!canMove)
        {
            return;
        }
        bool pushingWall = (theRB.velocity.x > 0 && coll.onRightWall) || (theRB.velocity.x < 0 && coll.onLeftWall);
        float push = pushingWall ? 0 : theRB.velocity.x;
        theRB.velocity = new Vector2(push, -slideSpeed);
        anim.ResetTrigger("doubleJump");
    }

    private void WallJump()
    {
        if ((transform.localScale.x == 1 && coll.onRightWall) || 
            transform.localScale.x == -1 && !coll.onRightWall)
        {
            Flip((int)(transform.localScale.x * -1));
        }
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));
        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;
        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);
        wallJumped = true;
    }

    private void WallParticle(float vertical)
    {
        var main = slideParticle.main;
        if (wallSlide ||
            (wallGrab && vertical < 0))
        {
            main.startColor = Color.white;
        }
        else
        {
            main.startColor = Color.clear;
        }
    }

    IEnumerator DashWait()
    {
        trail.ShowGhost();
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);
        dashParticle.Play();
        theRB.gravityScale = 0;
        wallJumped = true;
        isDashing = true;
        yield return new WaitForSeconds(1f);

        dashParticle.Stop();
        theRB.gravityScale = 3;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        hasDashed = !coll.onGround;
    }

    private void RigidbodyDrag(float x)
    {
        theRB.drag = x;
    }

    private int ParticleSide()
    {
        return coll.onRightWall ? 1 : -1;
    }

    private void UpdateAnim()
    {
        if (standing.activeSelf)
        {
            anim.SetBool("isOnGround", coll.onGround);
            anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        }
        if (ball.activeSelf)
        {
            ballAnim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        }
    }

    public void Flip(int side)
    {
        if (wallGrab || 
            wallSlide)
        {
            if (side == -1 &&
                transform.localScale.x==-1f)
            {
                return;
            }
            if (side == 1 &&
                transform.localScale.x == 1f)
            {
                return;
            }
        }
        transform.localScale = new Vector3(side, 1f, 1f);
    }

    public SpriteRenderer GetCurSprite()
    {
        if (standing.activeSelf)
        {
            return theSR;
        }
        return ballSR;
    }

    public void StopMove(bool isStop)
    {
        canMove = !isStop;
        theRB.velocity = Vector3.zero;
        anim.enabled = !isStop;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}