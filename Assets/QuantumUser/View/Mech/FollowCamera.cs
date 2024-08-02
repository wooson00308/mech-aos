using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Quantum.Mech
{
    public class FollowCamera : QuantumEntityViewComponent<CustomViewContext>
    {
        public GameObject camera;

        public override void OnActivate(Frame frame)
        {
            QuantumEvent.Subscribe<EventOnMechanicRespawn>(this, OnCameraSetting);
        }
        
        public void OnCameraSetting(EventOnMechanicRespawn respawn)
        {
            camera.gameObject.SetActive(respawn.Mechanic == EntityRef);
        }
  
    }
}

