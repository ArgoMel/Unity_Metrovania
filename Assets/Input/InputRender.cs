using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputRender : ScriptableObject
    , Platformer2d.IPlayerActions
    , Platformer2d.IUIActions
{
    private Platformer2d gameInput;

    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    public event Action JumpCancelledEvent;
    public event Action FireEvent;
    public event Action DashEvent;

    public event Action QuitMenuEvent;

    private void OnEnable()
    {
        if (gameInput==null) 
        {
            gameInput = new Platformer2d();
            gameInput.Player.SetCallbacks(this);
            gameInput.UI.SetCallbacks(this);
            SetGamePlay();
        }
    }

    public void SetGamePlay() 
    {
        gameInput.Player.Enable();
        gameInput.UI.Disable();
    }

    public void SetUI()
    {
        gameInput.Player.Disable();
        gameInput.UI.Enable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase==InputActionPhase.Performed) 
        {
            JumpEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            JumpCancelledEvent?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            FireEvent?.Invoke();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            DashEvent?.Invoke();
        }
    }

    public void OnQuitMenu(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            QuitMenuEvent?.Invoke();
            SetGamePlay();
        }
    }
}
