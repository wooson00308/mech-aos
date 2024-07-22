using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class MechSystem : SystemMainThreadFilter<MechSystem.Filter>, ISignalSpawnMechanic
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public PhysicsBody3D* Body;
            public PlayableMechanic* PlayableMechanic;
            public PlayerLink* PlayerLink;
        }
        public override void Update(Frame f, ref Filter filter)
        {

            if (f.Unsafe.TryGetPointer<Status>(filter.Entity, out var respawn))
            {
                respawn->RespawnTimer -= f.DeltaTime;
                if (respawn->RespawnTimer <= 0)
                {
                    f.Signals.AsteroidsSpawnShip(filter.Entity);
                }
                return;
            }
            
            var config = f.FindAsset(f.RuntimeConfig.MechGameConfig);
            Input* input = f.GetPlayerInput(filter.PlayerLink->PlayerRef);

            var velocity = UpdateMovement(f, ref filter, input, config);
            UpdateFire(f, ref filter, input, config);
            //UpdateRotate(f, ref filter, velocity);

        }
        private FPVector3 UpdateMovement(Frame f, ref Filter filter, Input* input, MechGameConfig config)
        {

            FPVector3 direction = FPVector3.Zero;
            if (input->Up)
            {
                direction += FPVector3.Forward;
            }
            if (input->Left)
            {
                direction += FPVector3.Left;
            }
            if (input->Right)
            {
                direction += FPVector3.Right;
            }
            if (input->Down)
            {
                direction += FPVector3.Back;
            }
            
            FP moveScale = FP._1;
            
            var velocity = direction.Normalized * config.MechMovementSpeed * f.DeltaTime;
            var combinedVelocity= velocity + new FPVector3(0,filter.Body->Velocity.Y,0);
            filter.Body->Velocity = combinedVelocity;
            
            return velocity;
        }
 
        private void UpdateFire(Frame f, ref Filter filter, Input* input, MechGameConfig config)
        {
            if (input->MainWeaponFire)
            {
                // f.Signals.AsteroidsShipShoot(filter.Entity);
                f.Events.WeaponFire(filter.Entity, EWeaponType.MainWeapon);
            }

            if (input->SubWeaponFire)
            {
                f.Events.WeaponFire(filter.Entity, EWeaponType.SubWeapon);
            }
        }
        public void SpawnMechanic(Frame f, EntityRef entity, QBoolean isLocal)
        {
            var config = f.FindAsset(f.RuntimeConfig.MechGameConfig);
            
            Transform3D* transform = f.Unsafe.GetPointer<Transform3D>(entity);
            transform->Position = MechUtils.GetRandomSpots(f, config.SpawnSpots);
            transform->Teleport(f, transform);

            var body = f.Unsafe.GetPointer<PhysicsBody3D>(entity);
            body->Velocity = default;
            body->AngularVelocity = default;
            // f.Unsafe.GetPointer<AsteroidsShip>(entity)->AmmoCount = config.MaxAmmo;
            f.Unsafe.GetPointer<PhysicsCollider3D>(entity)->Enabled = true;
        }
        
    }
}

