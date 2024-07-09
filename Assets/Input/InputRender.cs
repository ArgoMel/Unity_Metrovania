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
    public event Action<Vector2> MoveCanceledEvent;
    public event Action JumpEvent;
    public event Action JumpCanceledEvent;
    public event Action FireEvent;
    public event Action DashEvent;
    public event Action WallGrabEvent;
    public event Action WallGrabCanceledEvent;
    public event Action PauseEvent;
    public event Action OpenWorldMapEvent;
    public event Action ZoomInMapEvent;
    public event Action ZoomInMapCanceledEvent;
    public event Action ZoomOutMapEvent;
    public event Action ZoomOutMapCanceledEvent;

    public event Action ResumeEvent;

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
       // Debug.Log("SetGamePlay");
    }

    public void SetUI()
    {
        gameInput.Player.Disable();
        gameInput.UI.Enable();
       // Debug.Log("SetUI");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase==InputActionPhase.Performed) 
        {
            JumpEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            JumpCanceledEvent?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
        if (context.phase == InputActionPhase.Canceled)
        {
            MoveCanceledEvent?.Invoke(context.ReadValue<Vector2>());
        }
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

    public void OnWallGrab(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            WallGrabEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            WallGrabCanceledEvent?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
        }
    }

    public void OnOpenWorldMap(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OpenWorldMapEvent?.Invoke();
        }
    }

    public void OnZoomInMap(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ZoomInMapEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            ZoomInMapCanceledEvent?.Invoke();
        }
    }

    public void OnZoomOutMap(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ZoomOutMapEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            ZoomOutMapCanceledEvent?.Invoke();
        }
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ResumeEvent?.Invoke();
        }
    }
}
