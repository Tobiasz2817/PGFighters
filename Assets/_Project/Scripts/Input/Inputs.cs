//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/_Project/Scripts/Input/Inputs.inputactions
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

public partial class @Inputs : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Inputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs"",
    ""maps"": [
        {
            ""name"": ""Customize"",
            ""id"": ""175e6bb4-5151-4c07-b17c-0d338ac4c49f"",
            ""actions"": [
                {
                    ""name"": ""Load"",
                    ""type"": ""Button"",
                    ""id"": ""5667b035-5407-4982-aac2-f653218d76ec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Save"",
                    ""type"": ""Button"",
                    ""id"": ""9b9a5069-d984-4233-a700-0bd25d33bf66"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5f0d8f5a-5703-41d2-ab19-4ca0ddc271d3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Load"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d45f885e-a802-4611-acb2-6490267e0064"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Save"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""KeyboardCharacter"",
            ""id"": ""3591744f-7134-44d7-acb3-90d56a6e0994"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""549f3851-fc5e-4eb8-b366-efffa3113927"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""22c2b749-368f-46a6-8951-c2c23788d54c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TestShoot"",
                    ""type"": ""Button"",
                    ""id"": ""ce69c8e2-fc7b-42be-9593-16a3eebacdac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Dir"",
                    ""id"": ""edc789f9-86fb-4fae-ae8d-c90ac11a210e"",
                    ""path"": ""3DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0288eb72-3725-418e-94c7-d1a3faae41c3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7fffd015-ddd0-4a84-a08a-08d6e73b6b20"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""forward"",
                    ""id"": ""0ac02af9-6d94-4bd4-9790-45dab5c6ac50"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""backward"",
                    ""id"": ""46d16e47-d27b-48cf-b912-0eea8e05e950"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c6742dc1-fbca-4a19-95c0-3126b7b74288"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""31bc0bd8-926b-45fe-8096-946e2f50f90a"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TestShoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Environment"",
            ""id"": ""43822838-0dcd-4483-bc59-ede89e4a2da5"",
            ""actions"": [
                {
                    ""name"": ""Mouse"",
                    ""type"": ""Value"",
                    ""id"": ""b28fa51b-d125-464d-88e9-4fa3932df975"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b29fab76-ac07-455e-a971-163d33e1de15"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Mouse"",
            ""id"": ""1f198974-80b8-46c5-a756-a71ae94ba032"",
            ""actions"": [
                {
                    ""name"": ""Mouse"",
                    ""type"": ""Value"",
                    ""id"": ""a2ed788c-b3ec-4be1-8561-3a709176e500"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b8a4564f-9e8f-467a-b095-5cb34287f895"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Lobby"",
            ""id"": ""f72ef585-e8f3-434a-8540-4cdebf28ff51"",
            ""actions"": [
                {
                    ""name"": ""Exit"",
                    ""type"": ""Button"",
                    ""id"": ""df9ce567-0ae2-4f2d-91cc-5d0d450d38d0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7d516f22-dd6b-4b78-a61b-aba7f97ff76b"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Customize
        m_Customize = asset.FindActionMap("Customize", throwIfNotFound: true);
        m_Customize_Load = m_Customize.FindAction("Load", throwIfNotFound: true);
        m_Customize_Save = m_Customize.FindAction("Save", throwIfNotFound: true);
        // KeyboardCharacter
        m_KeyboardCharacter = asset.FindActionMap("KeyboardCharacter", throwIfNotFound: true);
        m_KeyboardCharacter_Movement = m_KeyboardCharacter.FindAction("Movement", throwIfNotFound: true);
        m_KeyboardCharacter_Shoot = m_KeyboardCharacter.FindAction("Shoot", throwIfNotFound: true);
        m_KeyboardCharacter_TestShoot = m_KeyboardCharacter.FindAction("TestShoot", throwIfNotFound: true);
        // Environment
        m_Environment = asset.FindActionMap("Environment", throwIfNotFound: true);
        m_Environment_Mouse = m_Environment.FindAction("Mouse", throwIfNotFound: true);
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_Mouse = m_Mouse.FindAction("Mouse", throwIfNotFound: true);
        // Lobby
        m_Lobby = asset.FindActionMap("Lobby", throwIfNotFound: true);
        m_Lobby_Exit = m_Lobby.FindAction("Exit", throwIfNotFound: true);
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

    // Customize
    private readonly InputActionMap m_Customize;
    private ICustomizeActions m_CustomizeActionsCallbackInterface;
    private readonly InputAction m_Customize_Load;
    private readonly InputAction m_Customize_Save;
    public struct CustomizeActions
    {
        private @Inputs m_Wrapper;
        public CustomizeActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Load => m_Wrapper.m_Customize_Load;
        public InputAction @Save => m_Wrapper.m_Customize_Save;
        public InputActionMap Get() { return m_Wrapper.m_Customize; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CustomizeActions set) { return set.Get(); }
        public void SetCallbacks(ICustomizeActions instance)
        {
            if (m_Wrapper.m_CustomizeActionsCallbackInterface != null)
            {
                @Load.started -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnLoad;
                @Load.performed -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnLoad;
                @Load.canceled -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnLoad;
                @Save.started -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnSave;
                @Save.performed -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnSave;
                @Save.canceled -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnSave;
            }
            m_Wrapper.m_CustomizeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Load.started += instance.OnLoad;
                @Load.performed += instance.OnLoad;
                @Load.canceled += instance.OnLoad;
                @Save.started += instance.OnSave;
                @Save.performed += instance.OnSave;
                @Save.canceled += instance.OnSave;
            }
        }
    }
    public CustomizeActions @Customize => new CustomizeActions(this);

    // KeyboardCharacter
    private readonly InputActionMap m_KeyboardCharacter;
    private IKeyboardCharacterActions m_KeyboardCharacterActionsCallbackInterface;
    private readonly InputAction m_KeyboardCharacter_Movement;
    private readonly InputAction m_KeyboardCharacter_Shoot;
    private readonly InputAction m_KeyboardCharacter_TestShoot;
    public struct KeyboardCharacterActions
    {
        private @Inputs m_Wrapper;
        public KeyboardCharacterActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_KeyboardCharacter_Movement;
        public InputAction @Shoot => m_Wrapper.m_KeyboardCharacter_Shoot;
        public InputAction @TestShoot => m_Wrapper.m_KeyboardCharacter_TestShoot;
        public InputActionMap Get() { return m_Wrapper.m_KeyboardCharacter; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyboardCharacterActions set) { return set.Get(); }
        public void SetCallbacks(IKeyboardCharacterActions instance)
        {
            if (m_Wrapper.m_KeyboardCharacterActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_KeyboardCharacterActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_KeyboardCharacterActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_KeyboardCharacterActionsCallbackInterface.OnMovement;
                @Shoot.started -= m_Wrapper.m_KeyboardCharacterActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_KeyboardCharacterActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_KeyboardCharacterActionsCallbackInterface.OnShoot;
                @TestShoot.started -= m_Wrapper.m_KeyboardCharacterActionsCallbackInterface.OnTestShoot;
                @TestShoot.performed -= m_Wrapper.m_KeyboardCharacterActionsCallbackInterface.OnTestShoot;
                @TestShoot.canceled -= m_Wrapper.m_KeyboardCharacterActionsCallbackInterface.OnTestShoot;
            }
            m_Wrapper.m_KeyboardCharacterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @TestShoot.started += instance.OnTestShoot;
                @TestShoot.performed += instance.OnTestShoot;
                @TestShoot.canceled += instance.OnTestShoot;
            }
        }
    }
    public KeyboardCharacterActions @KeyboardCharacter => new KeyboardCharacterActions(this);

    // Environment
    private readonly InputActionMap m_Environment;
    private IEnvironmentActions m_EnvironmentActionsCallbackInterface;
    private readonly InputAction m_Environment_Mouse;
    public struct EnvironmentActions
    {
        private @Inputs m_Wrapper;
        public EnvironmentActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Mouse => m_Wrapper.m_Environment_Mouse;
        public InputActionMap Get() { return m_Wrapper.m_Environment; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(EnvironmentActions set) { return set.Get(); }
        public void SetCallbacks(IEnvironmentActions instance)
        {
            if (m_Wrapper.m_EnvironmentActionsCallbackInterface != null)
            {
                @Mouse.started -= m_Wrapper.m_EnvironmentActionsCallbackInterface.OnMouse;
                @Mouse.performed -= m_Wrapper.m_EnvironmentActionsCallbackInterface.OnMouse;
                @Mouse.canceled -= m_Wrapper.m_EnvironmentActionsCallbackInterface.OnMouse;
            }
            m_Wrapper.m_EnvironmentActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Mouse.started += instance.OnMouse;
                @Mouse.performed += instance.OnMouse;
                @Mouse.canceled += instance.OnMouse;
            }
        }
    }
    public EnvironmentActions @Environment => new EnvironmentActions(this);

    // Mouse
    private readonly InputActionMap m_Mouse;
    private IMouseActions m_MouseActionsCallbackInterface;
    private readonly InputAction m_Mouse_Mouse;
    public struct MouseActions
    {
        private @Inputs m_Wrapper;
        public MouseActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Mouse => m_Wrapper.m_Mouse_Mouse;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void SetCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterface != null)
            {
                @Mouse.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnMouse;
                @Mouse.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnMouse;
                @Mouse.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnMouse;
            }
            m_Wrapper.m_MouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Mouse.started += instance.OnMouse;
                @Mouse.performed += instance.OnMouse;
                @Mouse.canceled += instance.OnMouse;
            }
        }
    }
    public MouseActions @Mouse => new MouseActions(this);

    // Lobby
    private readonly InputActionMap m_Lobby;
    private ILobbyActions m_LobbyActionsCallbackInterface;
    private readonly InputAction m_Lobby_Exit;
    public struct LobbyActions
    {
        private @Inputs m_Wrapper;
        public LobbyActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Exit => m_Wrapper.m_Lobby_Exit;
        public InputActionMap Get() { return m_Wrapper.m_Lobby; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LobbyActions set) { return set.Get(); }
        public void SetCallbacks(ILobbyActions instance)
        {
            if (m_Wrapper.m_LobbyActionsCallbackInterface != null)
            {
                @Exit.started -= m_Wrapper.m_LobbyActionsCallbackInterface.OnExit;
                @Exit.performed -= m_Wrapper.m_LobbyActionsCallbackInterface.OnExit;
                @Exit.canceled -= m_Wrapper.m_LobbyActionsCallbackInterface.OnExit;
            }
            m_Wrapper.m_LobbyActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Exit.started += instance.OnExit;
                @Exit.performed += instance.OnExit;
                @Exit.canceled += instance.OnExit;
            }
        }
    }
    public LobbyActions @Lobby => new LobbyActions(this);
    public interface ICustomizeActions
    {
        void OnLoad(InputAction.CallbackContext context);
        void OnSave(InputAction.CallbackContext context);
    }
    public interface IKeyboardCharacterActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnTestShoot(InputAction.CallbackContext context);
    }
    public interface IEnvironmentActions
    {
        void OnMouse(InputAction.CallbackContext context);
    }
    public interface IMouseActions
    {
        void OnMouse(InputAction.CallbackContext context);
    }
    public interface ILobbyActions
    {
        void OnExit(InputAction.CallbackContext context);
    }
}
