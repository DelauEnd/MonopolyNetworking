//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Game/Inputs/Controllers.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Controllers : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controllers()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controllers"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""edc35e3b-3e97-4cb7-be84-a397a5929a90"",
            ""actions"": [
                {
                    ""name"": ""UnlockCursor"",
                    ""type"": ""Button"",
                    ""id"": ""5e966c8e-4467-4f70-a8f1-ccb5bc6d9ee6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InspectItem"",
                    ""type"": ""Button"",
                    ""id"": ""47c64a2f-cd40-4dcc-8ba0-eb6191dae6c9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ShowTabMenu"",
                    ""type"": ""Button"",
                    ""id"": ""22c28413-de2c-4bf0-ac43-360e57988ec9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e75c0fb7-6767-4bb3-ad46-c1473af32860"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Hold(duration=0.1,pressPoint=1.401298E-45)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""UnlockCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""996551fd-de96-48db-9680-df0c72036725"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InspectItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0c1ae65-1364-464c-b25d-388f01a9076a"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": ""Hold(duration=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShowTabMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and mouse"",
            ""bindingGroup"": ""Keyboard and mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_UnlockCursor = m_Player.FindAction("UnlockCursor", throwIfNotFound: true);
        m_Player_InspectItem = m_Player.FindAction("InspectItem", throwIfNotFound: true);
        m_Player_ShowTabMenu = m_Player.FindAction("ShowTabMenu", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_UnlockCursor;
    private readonly InputAction m_Player_InspectItem;
    private readonly InputAction m_Player_ShowTabMenu;
    public struct PlayerActions
    {
        private @Controllers m_Wrapper;
        public PlayerActions(@Controllers wrapper) { m_Wrapper = wrapper; }
        public InputAction @UnlockCursor => m_Wrapper.m_Player_UnlockCursor;
        public InputAction @InspectItem => m_Wrapper.m_Player_InspectItem;
        public InputAction @ShowTabMenu => m_Wrapper.m_Player_ShowTabMenu;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @UnlockCursor.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUnlockCursor;
                @UnlockCursor.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUnlockCursor;
                @UnlockCursor.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUnlockCursor;
                @InspectItem.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInspectItem;
                @InspectItem.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInspectItem;
                @InspectItem.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInspectItem;
                @ShowTabMenu.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShowTabMenu;
                @ShowTabMenu.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShowTabMenu;
                @ShowTabMenu.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShowTabMenu;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @UnlockCursor.started += instance.OnUnlockCursor;
                @UnlockCursor.performed += instance.OnUnlockCursor;
                @UnlockCursor.canceled += instance.OnUnlockCursor;
                @InspectItem.started += instance.OnInspectItem;
                @InspectItem.performed += instance.OnInspectItem;
                @InspectItem.canceled += instance.OnInspectItem;
                @ShowTabMenu.started += instance.OnShowTabMenu;
                @ShowTabMenu.performed += instance.OnShowTabMenu;
                @ShowTabMenu.canceled += instance.OnShowTabMenu;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardandmouseSchemeIndex = -1;
    public InputControlScheme KeyboardandmouseScheme
    {
        get
        {
            if (m_KeyboardandmouseSchemeIndex == -1) m_KeyboardandmouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and mouse");
            return asset.controlSchemes[m_KeyboardandmouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnUnlockCursor(InputAction.CallbackContext context);
        void OnInspectItem(InputAction.CallbackContext context);
        void OnShowTabMenu(InputAction.CallbackContext context);
    }
}
