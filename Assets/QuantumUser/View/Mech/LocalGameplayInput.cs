using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Quantum.Mech
{
    public unsafe class LocalGameplayInput : QuantumSceneViewComponent<CustomViewContext>
    {
        private PlayerInput _playerInput;
        private Vector2 _lastPlayerDirection;
        
        public override void OnInitialize()
        {
            QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback),
                onlyIfActiveAndEnabled: true);
            _playerInput = GetComponent<PlayerInput>();
            ViewContext.LocalplayerInput = _playerInput;
        }

        public override void OnUpdateView()
        {
            // ViewContext.LocalPlayerLastDirection = GetAimDirection();
        }
        
        public void PollInput(CallbackPollInput callback)
        {
            Quantum.Input input = new Quantum.Input();

            if (callback.Game.GetLocalPlayers().Count == 0)
            {
                return;
            }
            
            
            // Note: Use GetKey() instead of GetKeyDown/Up. Quantum calculates up/down internally.
            Vector2 movementInput = _playerInput.actions["Movement"].ReadValue<Vector2>();
            /*
                Tertiary Action
                Quaternary Action
                Quinary Action
                Senary Action
                Septenary Action
                Octonary Action
                Nonary Action
                Denary Action
             */
        
            input.Movement = movementInput.ToFPVector2();
            input.MainWeaponFire = _playerInput.actions["Primary Action"].IsPressed();
            input.FirstSkill = _playerInput.actions["Secondary Action"].IsPressed();
            input.SecondSkill = _playerInput.actions["Tertiary Action"].IsPressed();
            input.ThirdSkill = _playerInput.actions["Quaternary Action"].IsPressed();
            input.Return = _playerInput.actions["Return Action"].IsPressed();
            input.ChangeWeapon = _playerInput.actions["Change Weapon Action"].IsPressed();


            input.MouseLeftButton = Mouse.current.leftButton.wasPressedThisFrame;
            var screenPosition = Mouse.current.position.ReadValue();
            input.MousePosition = screenPosition.ToFPVector2();
            
            callback.SetInput(input, DeterministicInputFlags.Repeatable);
        }
    }

}
