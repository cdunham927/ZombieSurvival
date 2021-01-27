// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerActions : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public PlayerActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActions"",
    ""maps"": [
        {
            ""name"": ""Player Controls"",
            ""id"": ""a856d966-211c-4995-ade9-a61ee99d036a"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""512172fc-f3a2-4b88-97f8-76018fc9b1d1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""3d5ef7d9-7fce-45da-b2d7-9bd4e4725751"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1f6fbe32-546e-4c35-8300-4471c64be982"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RMB"",
                    ""type"": ""Button"",
                    ""id"": ""d46fb556-3657-4c04-a049-dc3423ea6654"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""a44fe0dc-ee1a-4319-a7a0-ea1b78f7d252"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseScroll"",
                    ""type"": ""Button"",
                    ""id"": ""69875467-86f8-4b1c-b4fe-ee28e05320d9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""499ced1d-98b2-4ced-ba06-0554a32bd9af"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c6167429-14ad-4453-9ae7-2fcfa320bd3d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""86b5a165-3d73-4f30-86a7-2eefa2455d9a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e9420555-0480-4bee-8926-5f0540a61a08"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""14a325ea-3811-4de6-9dc0-95ac3ee4c36a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e3e5616c-3fdc-4a4c-a99e-d0c8fd2d3ee5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8b70a099-a55a-4150-9f1e-27b49f3257a3"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2608337-3459-47f4-94ad-cce391cf921e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RMB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d9da845-9a73-49ce-add0-8122e4c0024a"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b70a450c-1828-4a11-bc68-150fc07559c5"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MouseScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": []
        }
    ]
}");
        // Player Controls
        m_PlayerControls = asset.FindActionMap("Player Controls", throwIfNotFound: true);
        m_PlayerControls_Move = m_PlayerControls.FindAction("Move", throwIfNotFound: true);
        m_PlayerControls_Shoot = m_PlayerControls.FindAction("Shoot", throwIfNotFound: true);
        m_PlayerControls_Aim = m_PlayerControls.FindAction("Aim", throwIfNotFound: true);
        m_PlayerControls_RMB = m_PlayerControls.FindAction("RMB", throwIfNotFound: true);
        m_PlayerControls_Run = m_PlayerControls.FindAction("Run", throwIfNotFound: true);
        m_PlayerControls_MouseScroll = m_PlayerControls.FindAction("MouseScroll", throwIfNotFound: true);
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

    // Player Controls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Move;
    private readonly InputAction m_PlayerControls_Shoot;
    private readonly InputAction m_PlayerControls_Aim;
    private readonly InputAction m_PlayerControls_RMB;
    private readonly InputAction m_PlayerControls_Run;
    private readonly InputAction m_PlayerControls_MouseScroll;
    public struct PlayerControlsActions
    {
        private PlayerActions m_Wrapper;
        public PlayerControlsActions(PlayerActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerControls_Move;
        public InputAction @Shoot => m_Wrapper.m_PlayerControls_Shoot;
        public InputAction @Aim => m_Wrapper.m_PlayerControls_Aim;
        public InputAction @RMB => m_Wrapper.m_PlayerControls_RMB;
        public InputAction @Run => m_Wrapper.m_PlayerControls_Run;
        public InputAction @MouseScroll => m_Wrapper.m_PlayerControls_MouseScroll;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                Shoot.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnShoot;
                Shoot.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnShoot;
                Shoot.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnShoot;
                Aim.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAim;
                Aim.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAim;
                Aim.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAim;
                RMB.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRMB;
                RMB.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRMB;
                RMB.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRMB;
                Run.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRun;
                Run.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRun;
                Run.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRun;
                MouseScroll.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMouseScroll;
                MouseScroll.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMouseScroll;
                MouseScroll.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMouseScroll;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Shoot.started += instance.OnShoot;
                Shoot.performed += instance.OnShoot;
                Shoot.canceled += instance.OnShoot;
                Aim.started += instance.OnAim;
                Aim.performed += instance.OnAim;
                Aim.canceled += instance.OnAim;
                RMB.started += instance.OnRMB;
                RMB.performed += instance.OnRMB;
                RMB.canceled += instance.OnRMB;
                Run.started += instance.OnRun;
                Run.performed += instance.OnRun;
                Run.canceled += instance.OnRun;
                MouseScroll.started += instance.OnMouseScroll;
                MouseScroll.performed += instance.OnMouseScroll;
                MouseScroll.canceled += instance.OnMouseScroll;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IPlayerControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnRMB(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnMouseScroll(InputAction.CallbackContext context);
    }
}
