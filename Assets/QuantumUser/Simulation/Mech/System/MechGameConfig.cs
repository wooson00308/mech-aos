using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum.Mech
{
    public class MechGameConfig: AssetObject
    {
        [Header("Mech configuration")]
        [Tooltip("Prototype reference to spawn Mech")]
        public AssetRef<EntityPrototype> MechPrototype;
        
        [Header("configuration")]
        [Tooltip("메카의 움직임 스피드")] 
        public FP MechMovementSpeed;
        
        [Space]
        public FP lobbyingDuration = 10;
        public FP postgameDuration = 30;
        public FP centerTowerLatency = 5;
    }
}