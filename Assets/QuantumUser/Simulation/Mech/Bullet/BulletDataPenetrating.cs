using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Mech;
using UnityEngine;

namespace Quantum
{
    public class BulletDataPenetrating : BulletData
    {
        public FP penetratingCount;
        public List<EntityRef> hits = new ();
        public override unsafe void BulletAction(Frame frame, EntityRef bullet, EntityRef target, EHitTargetType hitTargetTyp)
        {
            var fields = frame.Unsafe.GetPointer<BulletFields>(bullet);
            var bulletIsOld = Duration > FP._0 && fields->Time >= Duration;
            if (bulletIsOld)
            {
                Debug.Log($"Laser : {fields->Time} / {Duration}");
                var position = frame.Unsafe.GetPointer<Transform3D>(bullet)->Position;
                frame.Events.OnBulletDestroyed(bullet.GetHashCode(), fields->Source, position, fields->Direction, fields->BulletData);
                frame.Destroy(bullet);
                return;
            }
            
            if (penetratingCount >= hits.Count) return;
            if (hits.Contains(target)) return;
            if (target == EntityRef.None) return;
            if (hitTargetTyp == EHitTargetType.None) return;
            
            Debug.Log($"{hitTargetTyp}");
            switch (hitTargetTyp)
            {
                case EHitTargetType.Mechanic:
                    frame.Signals.OnMechanicHit(bullet, target, Damage);
                    break;
                case EHitTargetType.Nexus:
                    frame.Signals.OnNexusHit(bullet, target, Damage);
                    break;
            }
            hits.Add(target);
        }
        
        
        public override unsafe void SpawnBullet(Frame frame, WeaponData weaponData, BulletData bulletData, EntityRef mechanic, FPVector3 direction)
        {
            var prototypeAsset = frame.FindAsset<EntityPrototype>(new AssetGuid(bulletData.BulletPrototype.Id.Value));
            var bullet = frame.Create(prototypeAsset);

            var bulletFields = frame.Unsafe.GetPointer<BulletFields>(bullet);
            var bulletTransform = frame.Unsafe.GetPointer<Transform3D>(bullet);
            bulletFields->BulletData = bulletData;
            
            var transform = frame.Unsafe.GetPointer<Transform3D>(mechanic);
            var fireSpotWorldOffset = WeaponHelper.GetFireSpotWorldOffset(
                weaponData,
                *transform,
                direction + (FPVector3.Forward * Range)
            );
            bulletTransform->Position = transform->Position + fireSpotWorldOffset;
            bulletFields->Direction = direction * weaponData.ShootForce;
            bulletTransform->Rotation = FPQuaternion.LookRotation(bulletFields->Direction);

            bulletFields->Source = mechanic;
            bulletFields->Time = FP._0;
        }
        
        public override unsafe void Move(Frame frame, EntityRef bullet)
        {
            
            // var fields = frame.Unsafe.GetPointer<BulletFields>(bullet);
            // var transform3D = frame.Unsafe.GetPointer<Transform3D>(bullet);
            // // transform3D->Position += fields->Direction * frame.DeltaTime;
        }

    }
}