using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class TrapSystem : SystemMainThreadFilter<TrapSystem.Filter>, ISignalOnGameEnded
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public TrapFields* TrapFields;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            var trap = filter.Entity;
            var trapTransform = filter.Transform;
            var trapFields = filter.TrapFields;
            var data = frame.FindAsset<TrapData>(trapFields->TrapData.Id);
            if (trapFields->DelayElapsedTime < data.Latency)
            {
                trapFields->DelayElapsedTime += frame.DeltaTime;
                return;
            }
            
            if (CheckRaycastCollision(frame, trap, *trapFields))
            {
                return;
            }
        }
        
        private bool CheckRaycastCollision(Frame frame, EntityRef trap, TrapFields trapFields)
        {
            Transform3D* trapTransform = frame.Unsafe.GetPointer<Transform3D>(trap);
            TrapData data = frame.FindAsset<TrapData>(trapFields.TrapData.Id);
            var shooter = frame.Unsafe.GetPointer<PlayableMechanic>(trapFields.Source);
            
                
            var hits = frame.Physics3D.OverlapShape(*trapTransform, data.ShapeConfig.CreateShape(frame));
            var isSuccess = false;
            for (var i = 0; i < hits.Count; i++)
            {
                var entity = hits[i].Entity;
                // if (entity == trapFields.Source) continue;
                if (entity == EntityRef.None || !frame.Has<Status>(entity)) continue;
                
                var playableMechanic = frame.Unsafe.GetPointer<PlayableMechanic>(entity);
                
                if (frame.Get<Status>(entity).IsDead)
                {
                    continue;
                }
                // Applies polymorphic behavior on the trap action
                data.Action(frame, trap, entity);
                isSuccess = true;
            }
            return isSuccess;
        }
        void ISignalOnGameEnded.OnGameEnded(Frame frame, GameController* gameController)
        {
            frame.SystemDisable<TrapSystem>();
        }

    }
}