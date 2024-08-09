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
        private CinemachineVirtualCamera _orbitalSupportCamera;
        public override void OnActivate(Frame frame)
        {
            base.OnActivate(frame);
            
            _localCamera.gameObject.SetActive(true);
            _orbitalSupportCamera.gameObject.SetActive(false);
            
            QuantumEvent.Subscribe(this, (EventOnMechanicOrbitalSupport e) => OnMechanicOrbitalSupport(e));
            QuantumEvent.Subscribe(this, (EventOnMechanicOrbitalSupportEnd e) => OnMechanicOrbitalSupportEnd(e));

        }
        public void OnMechanicOrbitalSupport(EventOnMechanicOrbitalSupport e)
        {
            _localCamera.gameObject.SetActive(false);
            _orbitalSupportCamera.gameObject.SetActive(true);
        }
        public void OnMechanicOrbitalSupportEnd(EventOnMechanicOrbitalSupportEnd e)
        {
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

