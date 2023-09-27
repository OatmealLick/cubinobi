//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Controls.inputactions
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

namespace Cubinobi.Project
{
    public partial class @Controls: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Controls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Game"",
            ""id"": ""f6fa2505-5119-4781-ae2d-b857afd23712"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""e9b911cd-0ea7-4771-bcb7-655276b582ec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""c1b20dfe-9497-4971-bba9-b4aad41f71b8"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""AttackMelee"",
                    ""type"": ""Button"",
                    ""id"": ""9bb9a7ba-2317-47ff-a7f2-279565504e28"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AttackRanged"",
                    ""type"": ""Button"",
                    ""id"": ""d6bffdc6-a6ef-44be-94bd-b1a158509039"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeStanceEarth"",
                    ""type"": ""Button"",
                    ""id"": ""76ef3c7d-9bc6-4a43-be94-914d4f83e9d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap(tapCount=3)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeStanceFire"",
                    ""type"": ""Button"",
                    ""id"": ""bc9a68b2-c86e-435b-9ad3-141b7b60afad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap(tapCount=3)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeStanceWind"",
                    ""type"": ""Button"",
                    ""id"": ""d791c3ba-beaa-4ae9-8710-e44acfd9bcd4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap(tapCount=3)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeStanceWater"",
                    ""type"": ""Button"",
                    ""id"": ""3a108acf-94ac-45de-aab6-5acc4051ac51"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap(tapCount=3)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeStanceVoid"",
                    ""type"": ""Button"",
                    ""id"": ""4aed5275-ce6d-4ca6-9792-0fc22468b323"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap(tapCount=3)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeStanceBasic"",
                    ""type"": ""Button"",
                    ""id"": ""2731bb08-dc90-409f-b0f5-0b99334790f3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap(tapCount=3)"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a3a213d1-f40b-4988-829c-51a863797619"",
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
                    ""id"": ""6f846334-f262-4fd7-b298-a4b8ee8966bd"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4135d080-7f73-4bc3-8129-2861748a99c1"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackMelee"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24a18852-b2b4-4091-ae49-1407c61fec76"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackMelee"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b2ccfad5-76bb-42bd-9e1e-ce654a5f1a0c"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackMelee"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2556e8e7-775a-4a73-929a-4cdac7e20fb4"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""b58d3119-ed42-4ba4-9852-9e16a01c9c46"",
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
                    ""id"": ""00da7756-7163-4397-9884-6395bde5d89e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4f02096f-8a18-4240-818c-2ca32d210e8e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""81b2a030-4052-4225-b213-4c7ec4099c2b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3f753857-b6d7-4291-8d5e-edda24a004d7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f1ac8b1d-8d52-4bde-9633-5e0b2dd3d8f3"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeStanceEarth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""51ee080a-bcc7-4228-969d-1d926fb09b72"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeStanceEarth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""86eabb40-4f66-4e0e-9cf3-cad044d3d543"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeStanceFire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""626a0525-8e87-4b6c-b03e-0c5ddba85d0b"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeStanceFire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""155af649-456a-4359-be2d-6e4d8997ca0a"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeStanceWind"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""817ff6a3-b634-4387-bf2d-2dbd0233577c"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeStanceWind"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""76f84c3d-2bb3-4be4-b0f2-63251dee8790"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeStanceWater"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""12598c77-4e24-494c-8a9d-7849d85879df"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeStanceWater"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b92c8871-5fb2-4c5d-84f9-0640da312888"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeStanceVoid"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa374c50-d8f6-4561-8fea-7726a4642336"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeStanceBasic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""294e8845-43f7-4ff3-b843-84c4bf4b928f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackRanged"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd171b1d-738e-4ad0-90fb-64bb17ed20c7"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackRanged"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2aac446-4553-4ff2-a965-bab57ce28001"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackRanged"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Game
            m_Game = asset.FindActionMap("Game", throwIfNotFound: true);
            m_Game_Jump = m_Game.FindAction("Jump", throwIfNotFound: true);
            m_Game_Move = m_Game.FindAction("Move", throwIfNotFound: true);
            m_Game_AttackMelee = m_Game.FindAction("AttackMelee", throwIfNotFound: true);
            m_Game_AttackRanged = m_Game.FindAction("AttackRanged", throwIfNotFound: true);
            m_Game_ChangeStanceEarth = m_Game.FindAction("ChangeStanceEarth", throwIfNotFound: true);
            m_Game_ChangeStanceFire = m_Game.FindAction("ChangeStanceFire", throwIfNotFound: true);
            m_Game_ChangeStanceWind = m_Game.FindAction("ChangeStanceWind", throwIfNotFound: true);
            m_Game_ChangeStanceWater = m_Game.FindAction("ChangeStanceWater", throwIfNotFound: true);
            m_Game_ChangeStanceVoid = m_Game.FindAction("ChangeStanceVoid", throwIfNotFound: true);
            m_Game_ChangeStanceBasic = m_Game.FindAction("ChangeStanceBasic", throwIfNotFound: true);
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

        // Game
        private readonly InputActionMap m_Game;
        private List<IGameActions> m_GameActionsCallbackInterfaces = new List<IGameActions>();
        private readonly InputAction m_Game_Jump;
        private readonly InputAction m_Game_Move;
        private readonly InputAction m_Game_AttackMelee;
        private readonly InputAction m_Game_AttackRanged;
        private readonly InputAction m_Game_ChangeStanceEarth;
        private readonly InputAction m_Game_ChangeStanceFire;
        private readonly InputAction m_Game_ChangeStanceWind;
        private readonly InputAction m_Game_ChangeStanceWater;
        private readonly InputAction m_Game_ChangeStanceVoid;
        private readonly InputAction m_Game_ChangeStanceBasic;
        public struct GameActions
        {
            private @Controls m_Wrapper;
            public GameActions(@Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Jump => m_Wrapper.m_Game_Jump;
            public InputAction @Move => m_Wrapper.m_Game_Move;
            public InputAction @AttackMelee => m_Wrapper.m_Game_AttackMelee;
            public InputAction @AttackRanged => m_Wrapper.m_Game_AttackRanged;
            public InputAction @ChangeStanceEarth => m_Wrapper.m_Game_ChangeStanceEarth;
            public InputAction @ChangeStanceFire => m_Wrapper.m_Game_ChangeStanceFire;
            public InputAction @ChangeStanceWind => m_Wrapper.m_Game_ChangeStanceWind;
            public InputAction @ChangeStanceWater => m_Wrapper.m_Game_ChangeStanceWater;
            public InputAction @ChangeStanceVoid => m_Wrapper.m_Game_ChangeStanceVoid;
            public InputAction @ChangeStanceBasic => m_Wrapper.m_Game_ChangeStanceBasic;
            public InputActionMap Get() { return m_Wrapper.m_Game; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GameActions set) { return set.Get(); }
            public void AddCallbacks(IGameActions instance)
            {
                if (instance == null || m_Wrapper.m_GameActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_GameActionsCallbackInterfaces.Add(instance);
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @AttackMelee.started += instance.OnAttackMelee;
                @AttackMelee.performed += instance.OnAttackMelee;
                @AttackMelee.canceled += instance.OnAttackMelee;
                @AttackRanged.started += instance.OnAttackRanged;
                @AttackRanged.performed += instance.OnAttackRanged;
                @AttackRanged.canceled += instance.OnAttackRanged;
                @ChangeStanceEarth.started += instance.OnChangeStanceEarth;
                @ChangeStanceEarth.performed += instance.OnChangeStanceEarth;
                @ChangeStanceEarth.canceled += instance.OnChangeStanceEarth;
                @ChangeStanceFire.started += instance.OnChangeStanceFire;
                @ChangeStanceFire.performed += instance.OnChangeStanceFire;
                @ChangeStanceFire.canceled += instance.OnChangeStanceFire;
                @ChangeStanceWind.started += instance.OnChangeStanceWind;
                @ChangeStanceWind.performed += instance.OnChangeStanceWind;
                @ChangeStanceWind.canceled += instance.OnChangeStanceWind;
                @ChangeStanceWater.started += instance.OnChangeStanceWater;
                @ChangeStanceWater.performed += instance.OnChangeStanceWater;
                @ChangeStanceWater.canceled += instance.OnChangeStanceWater;
                @ChangeStanceVoid.started += instance.OnChangeStanceVoid;
                @ChangeStanceVoid.performed += instance.OnChangeStanceVoid;
                @ChangeStanceVoid.canceled += instance.OnChangeStanceVoid;
                @ChangeStanceBasic.started += instance.OnChangeStanceBasic;
                @ChangeStanceBasic.performed += instance.OnChangeStanceBasic;
                @ChangeStanceBasic.canceled += instance.OnChangeStanceBasic;
            }

            private void UnregisterCallbacks(IGameActions instance)
            {
                @Jump.started -= instance.OnJump;
                @Jump.performed -= instance.OnJump;
                @Jump.canceled -= instance.OnJump;
                @Move.started -= instance.OnMove;
                @Move.performed -= instance.OnMove;
                @Move.canceled -= instance.OnMove;
                @AttackMelee.started -= instance.OnAttackMelee;
                @AttackMelee.performed -= instance.OnAttackMelee;
                @AttackMelee.canceled -= instance.OnAttackMelee;
                @AttackRanged.started -= instance.OnAttackRanged;
                @AttackRanged.performed -= instance.OnAttackRanged;
                @AttackRanged.canceled -= instance.OnAttackRanged;
                @ChangeStanceEarth.started -= instance.OnChangeStanceEarth;
                @ChangeStanceEarth.performed -= instance.OnChangeStanceEarth;
                @ChangeStanceEarth.canceled -= instance.OnChangeStanceEarth;
                @ChangeStanceFire.started -= instance.OnChangeStanceFire;
                @ChangeStanceFire.performed -= instance.OnChangeStanceFire;
                @ChangeStanceFire.canceled -= instance.OnChangeStanceFire;
                @ChangeStanceWind.started -= instance.OnChangeStanceWind;
                @ChangeStanceWind.performed -= instance.OnChangeStanceWind;
                @ChangeStanceWind.canceled -= instance.OnChangeStanceWind;
                @ChangeStanceWater.started -= instance.OnChangeStanceWater;
                @ChangeStanceWater.performed -= instance.OnChangeStanceWater;
                @ChangeStanceWater.canceled -= instance.OnChangeStanceWater;
                @ChangeStanceVoid.started -= instance.OnChangeStanceVoid;
                @ChangeStanceVoid.performed -= instance.OnChangeStanceVoid;
                @ChangeStanceVoid.canceled -= instance.OnChangeStanceVoid;
                @ChangeStanceBasic.started -= instance.OnChangeStanceBasic;
                @ChangeStanceBasic.performed -= instance.OnChangeStanceBasic;
                @ChangeStanceBasic.canceled -= instance.OnChangeStanceBasic;
            }

            public void RemoveCallbacks(IGameActions instance)
            {
                if (m_Wrapper.m_GameActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IGameActions instance)
            {
                foreach (var item in m_Wrapper.m_GameActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_GameActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public GameActions @Game => new GameActions(this);
        public interface IGameActions
        {
            void OnJump(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
            void OnAttackMelee(InputAction.CallbackContext context);
            void OnAttackRanged(InputAction.CallbackContext context);
            void OnChangeStanceEarth(InputAction.CallbackContext context);
            void OnChangeStanceFire(InputAction.CallbackContext context);
            void OnChangeStanceWind(InputAction.CallbackContext context);
            void OnChangeStanceWater(InputAction.CallbackContext context);
            void OnChangeStanceVoid(InputAction.CallbackContext context);
            void OnChangeStanceBasic(InputAction.CallbackContext context);
        }
    }
}
