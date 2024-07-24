using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum;
using UnityEngine;

namespace Quantum.Mech
{
    public unsafe class LocalGameplayInput : QuantumSceneViewComponent<CustomViewContext>
    {
        // private PlayerInput _playerInput;
        
        private Vector2 _lastPlayerDirection;
        
        public override void OnInitialize()
        {
            QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback),
                onlyIfActiveAndEnabled: true);
            // _playerInput = GetComponent<PlayerInput>();
        }

        public override void OnUpdateView()
        {
            // ViewContext.LocalPlayerLastDirection = GetAimDirection();
        }
        
        public void PollInput(CallbackPollInput callback)
        {
            Quantum.Input i = new Quantum.Input();

            if (callback.Game.GetLocalPlayers().Count == 0)
            {
                return;
            }
            
            // Note: Use GetKey() instead of GetKeyDown/Up. Quantum calculates up/down internally.
            i.Left = UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftArrow);
            i.Right = UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightArrow);
            i.Up = UnityEngine.Input.GetKey(KeyCode.W) || UnityEngine.Input.GetKey(KeyCode.UpArrow);
            i.Down = UnityEngine.Input.GetKey(KeyCode.S) || UnityEngine.Input.GetKey(KeyCode.DownArrow);
            
            
            i.MainWeaponFire = UnityEngine.Input.GetKey(KeyCode.K);
            i.SubWeaponFire = UnityEngine.Input.GetKey(KeyCode.L);
            
            callback.SetInput(i, DeterministicInputFlags.Repeatable);
        }
    }

}
