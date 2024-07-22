using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Quantum.Mech
{
    public unsafe class FollowCamera : QuantumSceneViewComponent
    {
        public CinemachineVirtualCamera Camera;

        public override void OnUpdateView()
        {
            if (Camera.Follow == null || Camera.LookAt == null)
            {
                var filter = VerifiedFrame.Filter<PlayableMechanic, PlayerLink>();
                while (filter.Next(out var entity, out var mechanic, out var playerLink))
                {
                    // if()
                }
            }
        }
    }
}

