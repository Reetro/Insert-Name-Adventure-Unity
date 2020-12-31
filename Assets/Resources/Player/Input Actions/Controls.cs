// GENERATED AUTOMATICALLY FROM 'Assets/Resources/Player/Input Actions/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace PlayerControls
{
    public class @Controls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Controls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""e27dbe24-c938-4411-ad48-953472f094ab"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""48ea1494-eb48-4df5-95a9-5409ad5f2f0b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""514f2139-dd61-49c4-b425-ce5fce9a04d7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""2b0ee4d7-ba5d-43b9-a6ce-c6bf8a75c93e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Mouse Postion"",
                    ""type"": ""Value"",
                    ""id"": ""47c1a039-93ad-4214-a577-ffe0e139cbb0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""18bf463d-0ca0-4133-bb1b-6a4263c06b78"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Save Game"",
                    ""type"": ""Button"",
                    ""id"": ""fe6830ac-955c-4944-8d2e-eafbfe4ff877"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Load Game"",
                    ""type"": ""Button"",
                    ""id"": ""f8260e4c-3153-4da9-88d9-2ada5110005f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Delete Saved Game"",
                    ""type"": ""Button"",
                    ""id"": ""b0c433bb-624c-4def-930d-ec4d1c441617"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Actionbar Slot 1"",
                    ""type"": ""Button"",
                    ""id"": ""86ca3fa6-8115-4ac8-82ad-507eb8e2151e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Actionbar Slot 2"",
                    ""type"": ""Button"",
                    ""id"": ""0a91e21c-458c-4a0c-85a8-bc1634948f56"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Actionbar Slot 3"",
                    ""type"": ""Button"",
                    ""id"": ""8db964fd-46b6-4ddf-8314-ddadd93eceb7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Actionbar Slot 4"",
                    ""type"": ""Button"",
                    ""id"": ""2e342863-d953-44e4-8a29-aefff002ee9c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ac97f5e8-6966-47af-9b2d-f99ff3a44923"",
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
                    ""id"": ""6dbc205c-67ff-4afa-b47e-cd5cbabf0273"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""3d06e454-ede8-44a2-beb0-03652f50c539"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7bd1a625-58ac-436c-96b4-9e14f581f002"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a545eaa2-fd4a-4805-911b-0b3505fb3af4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3d1cb94a-1e35-4d2b-aa84-43f07f3e312f"",
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
                    ""id"": ""16b43681-08d0-4f58-b3f1-5b4355302b16"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""1886cd80-1051-4504-92e0-f5052e535f9d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""35098933-93c8-4a37-9c21-88203a470d95"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c49d9c80-d93e-4987-aabc-984e44f6795b"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""12cba4b4-9f7f-4034-8286-550d32274cfa"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""821c0603-5e75-4687-9033-c8f92062456b"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""65012bb8-c745-4246-9351-36c58f25d007"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""549e01f5-5b26-402c-b9c1-9834d8e6caed"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45a11a7d-5781-49cd-ab4e-5b7623c3934d"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Postion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a1c7c07-b0ef-4ed5-8c6b-2f70c1f41546"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7a779b3-6a34-4e5d-ad36-505fe31e8e1f"",
                    ""path"": ""<Keyboard>/f6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Save Game"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a8060ea-66d4-4f66-9edd-1e1559306ea7"",
                    ""path"": ""<Keyboard>/f7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Load Game"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3abf08f-5567-458f-98f2-364755fa299a"",
                    ""path"": ""<Keyboard>/f8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete Saved Game"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58df8d3c-f45b-42a8-b654-3e98fa4e5214"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Actionbar Slot 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""088244d3-310b-4117-9179-f45d232a0e5a"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Actionbar Slot 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dfeb701c-d8c1-49ae-8141-d974a8ffd35b"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Actionbar Slot 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""caffdfd6-fc9d-4a12-a691-fa589f26c8f7"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Actionbar Slot 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4190b19-949d-4ea4-bfd0-c5ffbb505862"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Actionbar Slot 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9577b40d-6578-4c86-9b96-feb742b7cbef"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Actionbar Slot 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""acc9fc70-cfe7-4095-8491-13313f004e39"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Actionbar Slot 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95892808-c3cd-4356-a33e-98c8370887ce"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Actionbar Slot 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
            m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
            m_Player_Fire = m_Player.FindAction("Fire", throwIfNotFound: true);
            m_Player_MousePostion = m_Player.FindAction("Mouse Postion", throwIfNotFound: true);
            m_Player_Rotate = m_Player.FindAction("Rotate", throwIfNotFound: true);
            m_Player_SaveGame = m_Player.FindAction("Save Game", throwIfNotFound: true);
            m_Player_LoadGame = m_Player.FindAction("Load Game", throwIfNotFound: true);
            m_Player_DeleteSavedGame = m_Player.FindAction("Delete Saved Game", throwIfNotFound: true);
            m_Player_ActionbarSlot1 = m_Player.FindAction("Actionbar Slot 1", throwIfNotFound: true);
            m_Player_ActionbarSlot2 = m_Player.FindAction("Actionbar Slot 2", throwIfNotFound: true);
            m_Player_ActionbarSlot3 = m_Player.FindAction("Actionbar Slot 3", throwIfNotFound: true);
            m_Player_ActionbarSlot4 = m_Player.FindAction("Actionbar Slot 4", throwIfNotFound: true);
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

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_Jump;
        private readonly InputAction m_Player_Movement;
        private readonly InputAction m_Player_Fire;
        private readonly InputAction m_Player_MousePostion;
        private readonly InputAction m_Player_Rotate;
        private readonly InputAction m_Player_SaveGame;
        private readonly InputAction m_Player_LoadGame;
        private readonly InputAction m_Player_DeleteSavedGame;
        private readonly InputAction m_Player_ActionbarSlot1;
        private readonly InputAction m_Player_ActionbarSlot2;
        private readonly InputAction m_Player_ActionbarSlot3;
        private readonly InputAction m_Player_ActionbarSlot4;
        public struct PlayerActions
        {
            private @Controls m_Wrapper;
            public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Jump => m_Wrapper.m_Player_Jump;
            public InputAction @Movement => m_Wrapper.m_Player_Movement;
            public InputAction @Fire => m_Wrapper.m_Player_Fire;
            public InputAction @MousePostion => m_Wrapper.m_Player_MousePostion;
            public InputAction @Rotate => m_Wrapper.m_Player_Rotate;
            public InputAction @SaveGame => m_Wrapper.m_Player_SaveGame;
            public InputAction @LoadGame => m_Wrapper.m_Player_LoadGame;
            public InputAction @DeleteSavedGame => m_Wrapper.m_Player_DeleteSavedGame;
            public InputAction @ActionbarSlot1 => m_Wrapper.m_Player_ActionbarSlot1;
            public InputAction @ActionbarSlot2 => m_Wrapper.m_Player_ActionbarSlot2;
            public InputAction @ActionbarSlot3 => m_Wrapper.m_Player_ActionbarSlot3;
            public InputAction @ActionbarSlot4 => m_Wrapper.m_Player_ActionbarSlot4;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                    @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                    @Fire.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                    @Fire.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                    @Fire.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                    @MousePostion.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePostion;
                    @MousePostion.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePostion;
                    @MousePostion.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePostion;
                    @Rotate.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotate;
                    @Rotate.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotate;
                    @Rotate.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotate;
                    @SaveGame.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSaveGame;
                    @SaveGame.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSaveGame;
                    @SaveGame.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSaveGame;
                    @LoadGame.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLoadGame;
                    @LoadGame.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLoadGame;
                    @LoadGame.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLoadGame;
                    @DeleteSavedGame.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDeleteSavedGame;
                    @DeleteSavedGame.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDeleteSavedGame;
                    @DeleteSavedGame.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDeleteSavedGame;
                    @ActionbarSlot1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnActionbarSlot1;
                    @ActionbarSlot1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnActionbarSlot1;
                    @ActionbarSlot1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnActionbarSlot1;
                    @ActionbarSlot2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnActionbarSlot2;
                    @ActionbarSlot2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnActionbarSlot2;
                    @ActionbarSlot2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnActionbarSlot2;
                    @ActionbarSlot3.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnActionbarSlot3;
                    @ActionbarSlot3.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnActionbarSlot3;
                    @ActionbarSlot3.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnActionbarSlot3;
                    @ActionbarSlot4.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnActionbarSlot4;
                    @ActionbarSlot4.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnActionbarSlot4;
                    @ActionbarSlot4.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnActionbarSlot4;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                    @Fire.started += instance.OnFire;
                    @Fire.performed += instance.OnFire;
                    @Fire.canceled += instance.OnFire;
                    @MousePostion.started += instance.OnMousePostion;
                    @MousePostion.performed += instance.OnMousePostion;
                    @MousePostion.canceled += instance.OnMousePostion;
                    @Rotate.started += instance.OnRotate;
                    @Rotate.performed += instance.OnRotate;
                    @Rotate.canceled += instance.OnRotate;
                    @SaveGame.started += instance.OnSaveGame;
                    @SaveGame.performed += instance.OnSaveGame;
                    @SaveGame.canceled += instance.OnSaveGame;
                    @LoadGame.started += instance.OnLoadGame;
                    @LoadGame.performed += instance.OnLoadGame;
                    @LoadGame.canceled += instance.OnLoadGame;
                    @DeleteSavedGame.started += instance.OnDeleteSavedGame;
                    @DeleteSavedGame.performed += instance.OnDeleteSavedGame;
                    @DeleteSavedGame.canceled += instance.OnDeleteSavedGame;
                    @ActionbarSlot1.started += instance.OnActionbarSlot1;
                    @ActionbarSlot1.performed += instance.OnActionbarSlot1;
                    @ActionbarSlot1.canceled += instance.OnActionbarSlot1;
                    @ActionbarSlot2.started += instance.OnActionbarSlot2;
                    @ActionbarSlot2.performed += instance.OnActionbarSlot2;
                    @ActionbarSlot2.canceled += instance.OnActionbarSlot2;
                    @ActionbarSlot3.started += instance.OnActionbarSlot3;
                    @ActionbarSlot3.performed += instance.OnActionbarSlot3;
                    @ActionbarSlot3.canceled += instance.OnActionbarSlot3;
                    @ActionbarSlot4.started += instance.OnActionbarSlot4;
                    @ActionbarSlot4.performed += instance.OnActionbarSlot4;
                    @ActionbarSlot4.canceled += instance.OnActionbarSlot4;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        public interface IPlayerActions
        {
            void OnJump(InputAction.CallbackContext context);
            void OnMovement(InputAction.CallbackContext context);
            void OnFire(InputAction.CallbackContext context);
            void OnMousePostion(InputAction.CallbackContext context);
            void OnRotate(InputAction.CallbackContext context);
            void OnSaveGame(InputAction.CallbackContext context);
            void OnLoadGame(InputAction.CallbackContext context);
            void OnDeleteSavedGame(InputAction.CallbackContext context);
            void OnActionbarSlot1(InputAction.CallbackContext context);
            void OnActionbarSlot2(InputAction.CallbackContext context);
            void OnActionbarSlot3(InputAction.CallbackContext context);
            void OnActionbarSlot4(InputAction.CallbackContext context);
        }
    }
}
