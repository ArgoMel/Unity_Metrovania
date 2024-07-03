using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityTracker : MonoBehaviour
{
    public bool canDoubleJump;
    public bool canDash;
    public bool canBecomeBall;
    public bool canDropBomb;

    internal void UnlockAbility(PlayerAbility unlockAbility)
    {
        switch (unlockAbility)
        {
            case PlayerAbility.DoubleJump:
                canDoubleJump = true;
                break;
            case PlayerAbility.Dash:
                canDash = true;
                break;
            case PlayerAbility.BecomeBall:
                canBecomeBall = true;
                break;
            case PlayerAbility.DropBomb:
                canDropBomb = true; 
                break;  
        }
    }
}
