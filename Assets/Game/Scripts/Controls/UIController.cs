using Assets.Game.Scripts.UIHandlers.InGameUI;
using Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI;
using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class UIController : NetworkBehaviour
{
    [Header("User camera")]
    [SerializeField] private CinemachineFreeLook virtualCamera = null;
    [HideInInspector] UserFigure figure;
    [SerializeField] PlayerUIHandler PlayerUI;

    private Controllers controls;
    public Controllers Controls
    {
        get
        {
            return controls ??= controls = new Controllers();
        }
    }

    public override void OnStartAuthority()
    {
        Controls.Enable();

        figure = FindObjectsOfType<UserFigure>().FirstOrDefault(user=>user.hasAuthority);
        PlayerUI = GetComponent<PlayerUIHandler>();

        Controls.Player.UnlockCursor.started += OnUnlockCursorHold;
        Controls.Player.UnlockCursor.canceled += OnUnlockCursorRelease;

        Controls.Player.ShowTabMenu.started += ShowTabMenuHold;
        Controls.Player.ShowTabMenu.canceled += ShowTabMenuRelease; ;

        enabled = true;
    }

    public override void OnStopAuthority()
    {
        Controls.Disable();

        Controls.Player.UnlockCursor.started -= OnUnlockCursorHold;
        Controls.Player.UnlockCursor.canceled -= OnUnlockCursorRelease;

        Controls.Player.ShowTabMenu.started -= ShowTabMenuHold;
        Controls.Player.ShowTabMenu.canceled -= ShowTabMenuRelease; ;

        enabled = false;
    }

    private void ShowTabMenuHold(CallbackContext obj)
    {
        PlayerUI.TabMenuUI.ShowTabMenu(figure.Room.UserFigures);
    }

    private void ShowTabMenuRelease(CallbackContext obj)
    {
        PlayerUI.TabMenuUI.HideTabMenu();
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (hasAuthority)
            virtualCamera.gameObject.SetActive(false);
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (hasAuthority)
            virtualCamera.gameObject.SetActive(true);
    }
}
