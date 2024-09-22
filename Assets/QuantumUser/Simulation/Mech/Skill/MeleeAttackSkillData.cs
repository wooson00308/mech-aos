using Photon.Deterministic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum
{
    [Preserve]
    public unsafe class MeleeAttackSkillData  : SkillData
    {
        public AssetRef<MeleeAttackData> Data; 
        public override void Action(Frame frame, EntityRef mechanic)
        {
            if (!CheckRaycastCollision(frame, mechanic))
            {
                return;
            }
        }
        
        private bool CheckRaycastCollision(Frame frame, EntityRef mechanic)
        {
            Transform3D* transform = frame.Unsafe.GetPointer<Transform3D>(mechanic);
            var data = frame.FindAsset<MeleeAttackData>(Data.Id);
            var shooter = frame.Unsafe.GetPointer<PlayableMechanic>(mechanic);
            
            var hits = frame.Physics3D.OverlapShape(*transform, data.ShapeConfig.CreateShape(frame));
            for (var i = 0; i < hits.Count; i++)
            {
                var entity = hits[i].Entity;
                // 플레이어
                if (entity != EntityRef.None && frame.Has<Status>(entity) && entity != mechanic)
                {
                    var playableMechanic = frame.Unsafe.GetPointer<PlayableMechanic>(entity);
                    if (frame.Get<Status>(entity).IsDead || shooter->Team == playableMechanic->Team)
                    {
                        continue;
                    }
                    
                    data.Action(frame, mechanic, entity, EHitTargetType.Mechanic);
                    return true;
                }
                
                // 넥서스
                // if (entity != EntityRef.None && frame.Has<Nexus>(entity) && entity != mechanic)
                // {
                //     var nexus = frame.Get<Nexus>(entity);
                //     if (nexus.IsDestroy || shooter->Team == nexus.Team )
                //     {
                //         continue;
                //     }
                //     data.Action(frame, mechanic, entity, EHitTargetType.Nexus);
                //     return true;
                // }

            }
            return false;
        }
    }
}