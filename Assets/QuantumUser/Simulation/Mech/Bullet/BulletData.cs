using System.Collections;
using System.Collections.Generic;
using Quantum.Mech;
using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine;

    /// <summary>
    /// Polymorphic data asset for bullets
    /// </summary>
    public abstract class BulletData : AssetObject
    {
#if QUANTUM_UNITY
        [Header("View Configuration", order = 9)]
        public GameObject BulletDestroyFxGameObject;
        // public Blueless.AudioConfiguration BulletDestroyAudioInfo;
#endif
    
        public AssetRef<EntityPrototype> BulletPrototype;
        public Shape3DConfig ShapeConfig;
        public FP Damage;
        public FP Range;
        public FP Duration;

        /// <summary>
        /// Post hit effects should happen on this function call.
        /// 
        /// Eg.: Explosions, damage calculations, etc.
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="bullet">The bullet that triggered the function call</param>
        /// <param name="target">The target of the bullet (is null when hitting a static collider)</param>
        public virtual unsafe void BulletAction(Frame frame, EntityRef bullet, EntityRef target, EHitTargetType hitTargetType)
        {
        }

        public virtual unsafe void SpawnBullet(Frame frame, WeaponData weaponData, EntityRef mechanic, FPVector3 direction)
        {
            var prototypeAsset = frame.FindAsset<EntityPrototype>(new AssetGuid(BulletPrototype.Id.Value));
            var bullet = frame.Create(prototypeAsset);

            var bulletFields = frame.Unsafe.GetPointer<BulletFields>(bullet);
            var bulletTransform = frame.Unsafe.GetPointer<Transform3D>(bullet);
                
            bulletFields->BulletData = this;
            var transform = frame.Unsafe.GetPointer<Transform3D>(mechanic);
                
            var fireSpotWorldOffset = WeaponHelper.GetFireSpotWorldOffset(
                weaponData,
                *transform,
                direction
            );
                
            bulletTransform->Position = transform->Position + fireSpotWorldOffset;
            bulletFields->Direction = direction * weaponData.ShootForce;
            bulletTransform->Rotation = FPQuaternion.LookRotation(bulletFields->Direction);
            bulletFields->Source = mechanic;
            bulletFields->Time = FP._0;
        }

        public virtual unsafe void Move(Frame frame, EntityRef bullet)
        {
            var fields = frame.Unsafe.GetPointer<BulletFields>(bullet);
            var transform3D = frame.Unsafe.GetPointer<Transform3D>(bullet);
            transform3D->Position += fields->Direction * frame.DeltaTime;
        }
        
    }
}
