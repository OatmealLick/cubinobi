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
            _controls.Game.Jump.performed += HandleStartJump;
            _controls.Game.Jump.canceled += HandleStopJump;
            _controls.Game.Move.performed += HandleStartMove;
            _controls.Game.Move.canceled += HandleStopMove;
        }

        public void Dispose()
        {
            _controls.Game.Jump.performed -= HandleStartJump;
            _controls.Game.Jump.canceled -= HandleStopJump;
            _controls.Game.Move.performed -= HandleStartMove;
            _controls.Game.Move.canceled -= HandleStopMove;
        }

        private void HandleStartJump(InputAction.CallbackContext context)
        {
            _eventManager.SendEvent(new JumpStartEvent());
        }

        private void HandleStopJump(InputAction.CallbackContext context)
        {
            _eventManager.SendEvent(new JumpStopEvent());
        }

        private void HandleStartMove(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _eventManager.SendEvent(new StartMoveEvent(direction));
        }
        
        private void HandleStopMove(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _eventManager.SendEvent(new StopMoveEvent(direction));
        }
    }
    
    #region InputEvents

    public class JumpStopEvent : IEvent
    {
    }

    public class JumpStartEvent : IEvent
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