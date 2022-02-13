using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class UIController : NetworkBehaviour
{
    [Header("User camera")]
    [SerializeField] private CinemachineFreeLook virtualCamera = null;

    private Controls controls;
    public Controls Controls
    {
        get
        {
            return controls ??= controls = new Controls();
        }
    }

    public override void OnStartAuthority()
    {
        Controls.Enable();
        Controls.Player.UnlockCursor.started += OnUnlockCursorHold;
        Controls.Player.UnlockCursor.canceled += OnUnlockCursorRelease;

        enabled = true;
    }

    public void OnUnlockCursorHold(CallbackContext obj)
    {
        UnlockCursor();
    }

    private void OnUnlockCursorRelease(CallbackContext obj)
    {
        LockCursor();
    }

    public void UnlockCursor()
    {
        Debug.Log("performed");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        virtualCamera.gameObject.SetActive(false);
    }

    public void LockCursor()
    {
        Debug.Log("canceled");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        virtualCamera.gameObject.SetActive(true);
    }
}
