using Photon.Deterministic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Quantum
{
    public unsafe class TrapSkillData : SkillData
    {
        public AssetRef<EntityPrototype> trapPrototype;
        public AssetRef<TrapData> trapData; 
        
        public override void Action(Frame frame, EntityRef mechanic)
        {
            var prototypeAsset = frame.FindAsset<EntityPrototype>(new AssetGuid(trapPrototype.Id.Value));
            var trap = frame.Create(prototypeAsset);

            var trapFields = frame.Unsafe.GetPointer<TrapFields>(trap);
            var trapTransform = frame.Unsafe.GetPointer<Transform3D>(trap);
            var transform = frame.Unsafe.GetPointer<Transform3D>(mechanic);
            
            trapTransform->Position = transform->Position;
            trapFields->Source = mechanic;
            trapFields->TrapData = trapData;
            trapFields->DelayElapsedTime = FP._0;
        }
        
    }
}