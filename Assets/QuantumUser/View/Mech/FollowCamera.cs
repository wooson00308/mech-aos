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

