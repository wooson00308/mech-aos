using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum.Mech
{
    public class CustomViewContext : MonoBehaviour, IQuantumViewContext
    {
        // 자신 클라이언트 알아내는 컨텍스트
        public MechView LocalPlayerView;
        public FPVector3 LocalPlayerLastDirection;
    }

}
