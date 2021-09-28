// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset Asset { get; }

    public @InputMaster()
    {
        Asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""OnFoot"",
            ""id"": ""5511b314-2fd8-4c01-9d4e-f8107ba6c988"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""f2c25d3e-ea4f-466d-8d11-77b77da7558f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""5522a094-b699-4ea9-aaba-d75102ba36d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""05fa2c6a-3650-4283-980b-dfb9c8f53787"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c47ccd5d-9190-4d08-b541-ce6c39fabd01"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""6c8cbed8-3cbb-4875-aded-e9d694f754d1"",
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
                    ""id"": ""be83481e-8906-4931-92e7-7cc794e18b01"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5966a7f5-e74e-4f40-a543-6e5a3485846f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8df1c8fd-cc18-476a-b3e9-c23d6df10717"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""232ed503-e6c5-4344-9dcb-9da758352a53"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b505643a-2f58-44a0-af8b-b34d3e266b76"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f40166f-10af-4f3d-a7c7-ee58956537e1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2449b483-901d-4224-937a-9231f2e7fe08"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90c9b37c-9b95-4b67-a34a-60cf0b1f12db"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
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
        // OnFoot
        _mOnFoot = Asset.FindActionMap("OnFoot", true);
        _mOnFootMove = _mOnFoot.FindAction("Move", true);
        _mOnFootJump = _mOnFoot.FindAction("Jump", true);
        _mOnFootInteract = _mOnFoot.FindAction("Interact", true);
        _mOnFootCamera = _mOnFoot.FindAction("Camera", true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(Asset);
    }

    public InputBinding? bindingMask
    {
        get => Asset.bindingMask;
        set => Asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => Asset.devices;
        set => Asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => Asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return Asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return Asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        Asset.Enable();
    }

    public void Disable()
    {
        Asset.Disable();
    }

    // OnFoot
    private readonly InputActionMap _mOnFoot;
    private IOnFootActions _mOnFootActionsCallbackInterface;
    private readonly InputAction _mOnFootMove;
    private readonly InputAction _mOnFootJump;
    private readonly InputAction _mOnFootInteract;
    private readonly InputAction _mOnFootCamera;

    public struct OnFootActions
    {
        private @InputMaster _mWrapper;

        public OnFootActions(@InputMaster wrapper)
        {
            _mWrapper = wrapper;
        }

        public InputAction @Move => _mWrapper._mOnFootMove;
        public InputAction @Jump => _mWrapper._mOnFootJump;
        public InputAction @Interact => _mWrapper._mOnFootInteract;
        public InputAction @Camera => _mWrapper._mOnFootCamera;

        public InputActionMap Get()
        {
            return _mWrapper._mOnFoot;
        }

        public void Enable()
        {
            Get().Enable();
        }

        public void Disable()
        {
            Get().Disable();
        }

        public bool Enabled => Get().enabled;

        public static implicit operator InputActionMap(OnFootActions set)
        {
            return set.Get();
        }

        public void SetCallbacks(IOnFootActions instance)
        {
            if (_mWrapper._mOnFootActionsCallbackInterface != null)
            {
                @Move.started -= _mWrapper._mOnFootActionsCallbackInterface.OnMove;
                @Move.performed -= _mWrapper._mOnFootActionsCallbackInterface.OnMove;
                @Move.canceled -= _mWrapper._mOnFootActionsCallbackInterface.OnMove;
                @Jump.started -= _mWrapper._mOnFootActionsCallbackInterface.OnJump;
                @Jump.performed -= _mWrapper._mOnFootActionsCallbackInterface.OnJump;
                @Jump.canceled -= _mWrapper._mOnFootActionsCallbackInterface.OnJump;
                @Interact.started -= _mWrapper._mOnFootActionsCallbackInterface.OnInteract;
                @Interact.performed -= _mWrapper._mOnFootActionsCallbackInterface.OnInteract;
                @Interact.canceled -= _mWrapper._mOnFootActionsCallbackInterface.OnInteract;
                @Camera.started -= _mWrapper._mOnFootActionsCallbackInterface.OnCamera;
                @Camera.performed -= _mWrapper._mOnFootActionsCallbackInterface.OnCamera;
                @Camera.canceled -= _mWrapper._mOnFootActionsCallbackInterface.OnCamera;
            }

            _mWrapper._mOnFootActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
            }
        }
    }

    public OnFootActions @OnFoot => new OnFootActions(this);
    private int _mKeyboardandMouseSchemeIndex = -1;

    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (_mKeyboardandMouseSchemeIndex == -1)
                _mKeyboardandMouseSchemeIndex = Asset.FindControlSchemeIndex("Keyboard and Mouse");
            return Asset.controlSchemes[_mKeyboardandMouseSchemeIndex];
        }
    }

    public interface IOnFootActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
    }
}