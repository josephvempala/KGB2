// GENERATED AUTOMATICALLY FROM 'Assets/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public static InputMaster instance;
    public InputActionAsset asset { get; }
    private @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""GroundMovement"",
            ""id"": ""a8087ebb-b4f4-4e44-8a3c-20270ff1c562"",
            ""actions"": [
                {
                    ""name"": ""HorizontalMovement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a36a29aa-d367-45a7-80ae-3df9930f1114"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""7a18c3d0-9c3c-4fff-8415-36fe4abfcdb6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Walk"",
                    ""type"": ""Button"",
                    ""id"": ""6425ed46-bc67-43ba-b9c7-9f3a96619383"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""0dd6c355-cdde-4705-8315-f2e65dbaf19b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Prone"",
                    ""type"": ""Button"",
                    ""id"": ""023aa40e-3f08-429c-b226-65841e83e59f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StanceUp"",
                    ""type"": ""Button"",
                    ""id"": ""08c3803d-55ce-4030-9494-ada335ecdcb8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StanceDown"",
                    ""type"": ""Button"",
                    ""id"": ""1dc29d78-8f6a-4bca-a084-f085284398ec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseX"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f7a10283-8d1d-46e6-9355-bfb2bff03170"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseY"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e2de0ef2-acc8-484f-bbb4-1e079f5ebe8d"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""88594aff-ec53-4834-b80f-e71d7cf2a51b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""73c4bd1d-c560-4966-856f-d40cda7bf626"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""28e4b3ee-887c-41c0-8d1d-936e5578d93d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1bb199ba-ce4a-4556-9d33-c4d9c300a57a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3e8a494f-e631-4c1a-87ae-808f316641f2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a3195b01-7736-4833-a119-fe9a6974c50a"",
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
                    ""id"": ""badc498f-31a2-43f2-b363-19f5598c337e"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e202df03-29cc-4472-8dab-f8e7811161c0"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87c083c0-90b3-499f-87e7-2eb61271af31"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Prone"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""418d399a-90ae-4173-b649-1a2afc9ffd97"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StanceUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2608bb1a-1716-47da-a216-87665466c0ee"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StanceDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b12c9e88-110c-4a37-af46-3f31f73e4601"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""453055a1-a722-4119-93ca-65bdc6b5e37f"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GroundMovement
        m_GroundMovement = asset.FindActionMap("GroundMovement", throwIfNotFound: true);
        m_GroundMovement_HorizontalMovement = m_GroundMovement.FindAction("HorizontalMovement", throwIfNotFound: true);
        m_GroundMovement_Jump = m_GroundMovement.FindAction("Jump", throwIfNotFound: true);
        m_GroundMovement_Walk = m_GroundMovement.FindAction("Walk", throwIfNotFound: true);
        m_GroundMovement_Crouch = m_GroundMovement.FindAction("Crouch", throwIfNotFound: true);
        m_GroundMovement_Prone = m_GroundMovement.FindAction("Prone", throwIfNotFound: true);
        m_GroundMovement_StanceUp = m_GroundMovement.FindAction("StanceUp", throwIfNotFound: true);
        m_GroundMovement_StanceDown = m_GroundMovement.FindAction("StanceDown", throwIfNotFound: true);
        m_GroundMovement_MouseX = m_GroundMovement.FindAction("MouseX", throwIfNotFound: true);
        m_GroundMovement_MouseY = m_GroundMovement.FindAction("MouseY", throwIfNotFound: true);
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

    public static @InputMaster GetInstance()
    {
        if(instance == null)
        {
            instance = new InputMaster();
            return instance;
        }
        else
        {
            return instance;
        }
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

    // GroundMovement
    private readonly InputActionMap m_GroundMovement;
    private IGroundMovementActions m_GroundMovementActionsCallbackInterface;
    private readonly InputAction m_GroundMovement_HorizontalMovement;
    private readonly InputAction m_GroundMovement_Jump;
    private readonly InputAction m_GroundMovement_Walk;
    private readonly InputAction m_GroundMovement_Crouch;
    private readonly InputAction m_GroundMovement_Prone;
    private readonly InputAction m_GroundMovement_StanceUp;
    private readonly InputAction m_GroundMovement_StanceDown;
    private readonly InputAction m_GroundMovement_MouseX;
    private readonly InputAction m_GroundMovement_MouseY;
    public struct GroundMovementActions
    {
        private @InputMaster m_Wrapper;
        public GroundMovementActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @HorizontalMovement => m_Wrapper.m_GroundMovement_HorizontalMovement;
        public InputAction @Jump => m_Wrapper.m_GroundMovement_Jump;
        public InputAction @Walk => m_Wrapper.m_GroundMovement_Walk;
        public InputAction @Crouch => m_Wrapper.m_GroundMovement_Crouch;
        public InputAction @Prone => m_Wrapper.m_GroundMovement_Prone;
        public InputAction @StanceUp => m_Wrapper.m_GroundMovement_StanceUp;
        public InputAction @StanceDown => m_Wrapper.m_GroundMovement_StanceDown;
        public InputAction @MouseX => m_Wrapper.m_GroundMovement_MouseX;
        public InputAction @MouseY => m_Wrapper.m_GroundMovement_MouseY;
        public InputActionMap Get() { return m_Wrapper.m_GroundMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GroundMovementActions set) { return set.Get(); }
        public void SetCallbacks(IGroundMovementActions instance)
        {
            if (m_Wrapper.m_GroundMovementActionsCallbackInterface != null)
            {
                @HorizontalMovement.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnHorizontalMovement;
                @HorizontalMovement.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnHorizontalMovement;
                @HorizontalMovement.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnHorizontalMovement;
                @Jump.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnJump;
                @Walk.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnWalk;
                @Walk.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnWalk;
                @Walk.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnWalk;
                @Crouch.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnCrouch;
                @Prone.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnProne;
                @Prone.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnProne;
                @Prone.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnProne;
                @StanceUp.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnStanceUp;
                @StanceUp.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnStanceUp;
                @StanceUp.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnStanceUp;
                @StanceDown.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnStanceDown;
                @StanceDown.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnStanceDown;
                @StanceDown.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnStanceDown;
                @MouseX.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMouseX;
                @MouseX.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMouseX;
                @MouseX.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMouseX;
                @MouseY.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMouseY;
                @MouseY.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMouseY;
                @MouseY.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMouseY;
            }
            m_Wrapper.m_GroundMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HorizontalMovement.started += instance.OnHorizontalMovement;
                @HorizontalMovement.performed += instance.OnHorizontalMovement;
                @HorizontalMovement.canceled += instance.OnHorizontalMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Walk.started += instance.OnWalk;
                @Walk.performed += instance.OnWalk;
                @Walk.canceled += instance.OnWalk;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @Prone.started += instance.OnProne;
                @Prone.performed += instance.OnProne;
                @Prone.canceled += instance.OnProne;
                @StanceUp.started += instance.OnStanceUp;
                @StanceUp.performed += instance.OnStanceUp;
                @StanceUp.canceled += instance.OnStanceUp;
                @StanceDown.started += instance.OnStanceDown;
                @StanceDown.performed += instance.OnStanceDown;
                @StanceDown.canceled += instance.OnStanceDown;
                @MouseX.started += instance.OnMouseX;
                @MouseX.performed += instance.OnMouseX;
                @MouseX.canceled += instance.OnMouseX;
                @MouseY.started += instance.OnMouseY;
                @MouseY.performed += instance.OnMouseY;
                @MouseY.canceled += instance.OnMouseY;
            }
        }
    }
    public GroundMovementActions @GroundMovement => new GroundMovementActions(this);
    public interface IGroundMovementActions
    {
        void OnHorizontalMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnWalk(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnProne(InputAction.CallbackContext context);
        void OnStanceUp(InputAction.CallbackContext context);
        void OnStanceDown(InputAction.CallbackContext context);
        void OnMouseX(InputAction.CallbackContext context);
        void OnMouseY(InputAction.CallbackContext context);
    }
}
