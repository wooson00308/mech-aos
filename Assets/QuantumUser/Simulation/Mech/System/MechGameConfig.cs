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


        [Header("Map configuration")] 
        [Tooltip("스폰 위치")]
        public List<FPVector3> SpawnSpots;





        // [Header("Map configuration")]
        // [Tooltip("Total size of the map. This is used to calculate when an entity is outside de gameplay area and then wrap it to the other side")]
        // public FPVector3 GameMapSize = new FPVector3(25, 25, 25);
        // public FPVector3 MapExtends => _mapExtends;
        // private FPVector3 _mapExtends;
        //
        // public override void Loaded(IResourceManager resourceManager, Native.Allocator allocator)
        // {
        //     base.Loaded(resourceManager, allocator);
        //
        //     _mapExtends = GameMapSize / 2;
        // }
    }
}