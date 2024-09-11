using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Quantum.Mech
{
    public class FollowCamera : QuantumSceneViewComponent<CustomViewContext>
    {
        [SerializeField]
        private CinemachineVirtualCamera _localCamera;
        
        [SerializeField]
        private CinemachineVirtualCamera _introCamera;
        
        [SerializeField] 
        private CinemachineVirtualCamera _orbitalSupportCamera;

        public void Awake()
        {
            QuantumEvent.Subscribe(this, (EventGameStateChanged e) => OnGameStateChanged(e));
            QuantumEvent.Subscribe(this, (EventOnMechanicOrbitalSupport e) => OnMechanicOrbitalSupport(e));
            QuantumEvent.Subscribe(this, (EventOnMechanicOrbitalSupportEnd e) => OnMechanicOrbitalSupportEnd(e));
        }

        public override void OnActivate(Frame frame)
        {
            base.OnActivate(frame);
            
            _introCamera.gameObject.SetActive(true);
            _localCamera.gameObject.SetActive(false);
            _orbitalSupportCamera.gameObject.SetActive(false);

        }

        public void OnGameStateChanged(EventGameStateChanged e)
        {
            if (e.NewState == GameState.Game)
            {
                _introCamera.gameObject.SetActive(false);
                _localCamera.gameObject.SetActive(true);
                _orbitalSupportCamera.gameObject.SetActive(false);
            }
        }
        public void OnMechanicOrbitalSupport(EventOnMechanicOrbitalSupport e)
        {
            if (e.Mechanic != ViewContext.EntityRef) return;
            _introCamera.gameObject.SetActive(false);
            _localCamera.gameObject.SetActive(false);
            _orbitalSupportCamera.gameObject.SetActive(true);

        }
        public void OnMechanicOrbitalSupportEnd(EventOnMechanicOrbitalSupportEnd e)
        {
            if (e.Mechanic != ViewContext.EntityRef) return;
            _introCamera.gameObject.SetActive(false);
            _localCamera.gameObject.SetActive(true);
            _orbitalSupportCamera.gameObject.SetActive(false);
        }
        
        public override void OnUpdateView()
        {
            if (ViewContext.LocalPlayerView == null)
            {
                return;
            }

            if (_localCamera.Follow == null)
                _localCamera.Follow = ViewContext.LocalPlayerView.transform;

        }

    }
}

