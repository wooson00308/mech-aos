using Photon.Deterministic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class BulletSystem : SystemMainThreadFilter<BulletSystem.Filter>, ISignalOnGameEnded
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public BulletFields* BulletFields;
        }
        void ISignalOnGameEnded.OnGameEnded(Frame frame, GameController* gameController)
        {
            frame.SystemDisable<BulletSystem>();
        }
        
        public override void Update(Frame frame, ref Filter filter)
        {
            var bullet = filter.Entity;
            var bulletTransform = filter.Transform;
            var bulletFields = filter.BulletFields;
      
            if (CheckRaycastCollision(frame, bullet, *bulletFields))
            {
                return;
            }
            FPVector3 sourcePosition = frame.Unsafe.GetPointer<Transform3D>(bulletFields->Source)->Position;
            BulletData bulletData = frame.FindAsset<BulletData>(bulletFields->BulletData.Id);
            
            bulletData.Move(frame, bullet);
            bulletFields->Time += frame.DeltaTime;
            
            FP distanceSquared = FPVector3.DistanceSquared(bulletTransform->Position, sourcePosition);
            bool bulletIsTooFar = FPMath.Sqrt(distanceSquared) > bulletData.Range;
            bool bulletIsOld = bulletData.Duration > FP._0 && bulletFields->Time >= bulletData.Duration;

            if (bulletIsTooFar || bulletIsOld)
            {
                // Applies polymorphic behavior on the bullet action
                bulletData.BulletAction(frame, bullet, EntityRef.None, EHitTargetType.None);
            }
        }
        
        private bool CheckRaycastCollision(Frame frame, EntityRef bullet, BulletFields bulletFields)
        {
            if (bulletFields.Direction.Magnitude <= 0)
            {
                return false;
            }

            Transform3D* bulletTransform = frame.Unsafe.GetPointer<Transform3D>(bullet);
            FPVector3 futurePosition = bulletTransform->Position + bulletFields.Direction * frame.DeltaTime;
            BulletData data = frame.FindAsset<BulletData>(bulletFields.BulletData.Id);
            
            if (FPVector3.DistanceSquared(bulletTransform->Position, futurePosition) <= FP._0_01)
            {
                return false;
            }
            
            var shooter = frame.Unsafe.GetPointer<PlayableMechanic>(bulletFields.Source);
            
                
            var hits = frame.Physics3D.OverlapShape(*bulletTransform, data.ShapeConfig.CreateShape(frame));
            for (int i = 0; i < hits.Count; i++)
            {
                var entity = hits[i].Entity;
                
                
                // 플레이어
                if (entity != EntityRef.None && frame.Has<Status>(entity) && entity != bulletFields.Source)
                {
                    var playableMechanic = frame.Unsafe.GetPointer<PlayableMechanic>(entity);
                    if (frame.Get<Status>(entity).IsDead || shooter->Team == playableMechanic->Team)
                    {
                        continue;
                    }

                    bulletTransform->Position = hits[i].Point;
                    // Applies polymorphic behavior on the bullet action
                    data.BulletAction(frame, bullet, entity, EHitTargetType.Mechanic);
                    return true;
                }
                
                // 넥서스
                if (entity != EntityRef.None && frame.Has<Nexus>(entity) && entity != bulletFields.Source)
                {
                    var nexus = frame.Get<Nexus>(entity);
                    if (nexus.IsDestroy || shooter->Team == nexus.Team )
                    {
                        continue;
                    }

                    bulletTransform->Position = hits[i].Point;
                    data.BulletAction(frame, bullet, entity, EHitTargetType.Nexus);
                    return true;
                }
                
                if (entity == EntityRef.None)
                {
                    // var map = frame.FindAsset(hits[i].StaticColliderIndex);
                    if (hits[i].IsStatic)
                    {
                        var staticCollider = frame.Map.StaticColliders3D[hits[i].StaticColliderIndex];
                        var projectilePassageLayer = frame.Layers.GetLayerMask("ProjectilePassageEnvironment");
                        if ((1 << staticCollider.StaticData.Layer) == projectilePassageLayer)
                        {
                            return false;
                        }
                    }
                    bulletTransform->Position = hits[i].Point;
                    data.BulletAction(frame, bullet, EntityRef.None, EHitTargetType.None);
                    return true;
                }
            }
            return false;
        }
    }
}