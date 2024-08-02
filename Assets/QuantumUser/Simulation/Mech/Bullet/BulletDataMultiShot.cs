using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Mech;
using UnityEngine;

namespace Quantum
{
    public class BulletDataMultiShot : BulletDataCommon
    {
        public FP projectileCount;
        public FP projectileGap;
        public FP circleRadius;
        public FP hemisphereAngle ;
        //     PLANE 미구현
        public ESpreadShape spreadShape;
        //     VERTICAL, Grid 미구현
        public ESpreadDirection spreadDirection;

        public override unsafe void SpawnBullet(Frame frame, WeaponData weaponData, BulletData bulletData, EntityRef mechanic, FPVector3 direction)
        {
            if (spreadShape == ESpreadShape.PLANE)
            {
                ShootInPlane(frame, weaponData, bulletData, mechanic, direction);
            }
            else if (spreadShape == ESpreadShape.CIRCULAR)
            {
                ShootInCircle(frame, weaponData, bulletData, mechanic, direction);
            }
            
        }
        private void ShootInPlane(Frame frame, WeaponData weaponData, BulletData bulletData, EntityRef mechanic, FPVector3 direction)
        {
            FPVector3 orthogonalDirection;

            // Determine orthogonal direction based on spreadDirection
            if (spreadDirection == ESpreadDirection.HORIZONTAL)
            {
                orthogonalDirection = FPVector3.Cross(direction, FPVector3.Up).Normalized;
            }
            else if (spreadDirection == ESpreadDirection.VERTICAL)
            {
                orthogonalDirection = FPVector3.Cross(direction, FPVector3.Right).Normalized;
            }
            else
            {
                orthogonalDirection = FPVector3.Right; // Default case
            }

            for (int i = 0; i < projectileCount; i++)
            {
                var offset = orthogonalDirection * (i - (projectileCount - 1) / FP.FromFloat_UNSAFE(2.0f) ) * projectileGap;
                CreateAndShootBullet(frame, weaponData, bulletData, mechanic, direction, offset);
            }
        }
        private void ShootInCircle(Frame frame, WeaponData weaponData, BulletData bulletData, EntityRef mechanic, FPVector3 direction)
        {
            
            FP angleStep =  hemisphereAngle / projectileCount;
            FPVector3 forward = direction.Normalized;

            for (int i = 0; i < projectileCount; i++)
            {
                FP angle = -(hemisphereAngle / 2) + (i * angleStep);
                FPVector3 rotatedDirection = RotateVector(forward, angle).Normalized;
                FPVector3 offset = rotatedDirection * circleRadius;
                CreateAndShootBullet(frame, weaponData, bulletData, mechanic, rotatedDirection, offset);
            }
        }
        private FPVector3 RotateVector(FPVector3 vector, FP angle)
        {
            var rad = angle * FP.Deg2Rad;
            var sin = FPMath.Sin(rad);
            var cos = FPMath.Cos(rad);

            var x = vector.X * cos - vector.Z * sin;
            var z = vector.X * sin + vector.Z * cos;

            return new FPVector3(x, vector.Y, z);
        }
        private unsafe void CreateAndShootBullet(Frame frame, WeaponData weaponData, BulletData bulletData, EntityRef mechanic, FPVector3 direction, FPVector3 offset)
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
                direction
            );

            bulletTransform->Position = transform->Position + fireSpotWorldOffset + offset;
            bulletFields->Direction = direction * weaponData.ShootForce;
            bulletTransform->Rotation = FPQuaternion.LookRotation(bulletFields->Direction);
            bulletFields->Source = mechanic;
            bulletFields->Time = FP._0;
        }
    }
}
