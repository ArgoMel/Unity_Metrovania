using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;
    [Space]
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;
    [Space]
    [Header("Collision")]
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset;
    public Vector2 rightOffset;
    public Vector2 leftOffset;

    public event Action GroundTouchEvent;
    public event Action WallOutEvent;

    private void FixedUpdate()
    {
        bool touchGround = 
            Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        if (touchGround&&
            !onGround)
        {
            GroundTouchEvent?.Invoke();
        }
        onGround = touchGround;

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        bool wallout = onRightWall || onLeftWall;
        if (!wallout &&
            onWall)
        {
            WallOutEvent?.Invoke();
        }
        onWall = wallout;

        wallSide = onRightWall ? -1 : 1;
    }

    void OnDrawGizmos()
    {
        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };
        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
