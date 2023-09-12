using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Cubinobi.Project
{
    public class InputManager : IInitializable, IDisposable
    {
        private readonly EventManager _eventManager;
        private readonly Controls _controls;

        public InputManager(EventManager eventManager)
        {
            _eventManager = eventManager;
            _controls = new Controls();
        }

        public void Initialize()
        {
            _controls.Game.Enable();
            _controls.Game.Jump.performed += HandleJump;
            _controls.Game.Move.performed += HandleStartMove;
            _controls.Game.Move.canceled += HandleStopMove;
        }

        public void Dispose()
        {
            _controls.Game.Jump.performed -= HandleJump;
            _controls.Game.Move.performed -= HandleStartMove;
            _controls.Game.Move.canceled -= HandleStopMove;
        }

        private void HandleJump(InputAction.CallbackContext context)
        {
            Debug.Log("jump");
            _eventManager.SendEvent(new JumpEvent());
        }

        private void HandleStartMove(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            Debug.Log($"start move {direction}");
            _eventManager.SendEvent(new StartMoveEvent(direction));
        }
        
        private void HandleStopMove(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            Debug.Log($"stop move {direction}");
            _eventManager.SendEvent(new StopMoveEvent(direction));
        }
    }
    
    #region InputEvents

    public class JumpEvent : IEvent
    {
    }

    public class StartMoveEvent : IEvent
    {
        public Vector2 Direction { get; }

        public StartMoveEvent(Vector2 direction)
        {
            Direction = direction;
        }
    }
    
    public class StopMoveEvent : IEvent
    {
        public Vector2 Direction { get; }

        public StopMoveEvent(Vector2 direction)
        {
            Direction = direction;
        }
    }
    
    #endregion
}