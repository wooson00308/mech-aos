using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Quantum.Mech
{
    public class CustomViewContext : MonoBehaviour, IQuantumViewContext
    {
        // 자신 클라이언트 알아내는 컨텍스트
        public MechView LocalPlayerView;
        public EntityRef LocalEntityRef;
        public PlayerInput LocalplayerInput;
        public FPVector3 LocalPlayerLastDirection;
        
    }

}
