using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Core;
using Quantum.Physics2D;
using Quantum.Physics3D;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter>, 
        ISignalOnGameEnded, IKCCCallbacks3D
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public CharacterController3D* KCC;
            public PlayableMechanic* PlayableMechanic;
            public PlayerLink* PlayerLink;
            public Status* Status;
            public AbilityInventory* AbilityInventory;
            
        }
        public void OnGameEnded(Frame frame, GameController* gameController)
        {
            foreach (var (mechanic, kcc) in frame.Unsafe.GetComponentBlockIterator<CharacterController3D>())
            {
                kcc->Velocity = FPVector3.Zero;
            }

            frame.SystemDisable<MovementSystem>();
        }
        public override void Update(Frame f, ref Filter filter)
        {
            var entity = filter.Entity;
            var status = filter.Status;

            var config = f.FindAsset(f.RuntimeConfig.MechGameConfig);
            Input* input = f.GetPlayerInput(filter.PlayerLink->PlayerRef);
            bool wasGrounded = filter.KCC->Grounded;
            bool hasActiveAbility = filter.AbilityInventory->TryGetActiveAbility(out Ability activeAbility);
            AbilityData activeAbilityData = null;
            
            if (hasActiveAbility)
            {
                activeAbilityData = f.FindAsset<AbilityData>(activeAbility.AbilityData.Id);
            }
            
            PlayerMovementData movementData = f.FindAsset<PlayerMovementData>(filter.Status->PlayerMovementData.Id);
            
            if ((hasActiveAbility && !activeAbilityData.KeepVelocity) || status->IsDead)
            {
                // filter.KCC->Velocity = FPVector3.Lerp(filter.KCC->Velocity, FPVector3.Zero, movementData.NoMovementBraking * f.DeltaTime);
                filter.KCC->Velocity = FPVector3.Zero;
            }
            
            FPVector3 movementDirection;
            if (hasActiveAbility || status->IsDead)
            {
                movementDirection = FPVector3.Zero;
            }
            else
            {
                movementDirection = input->Movement.XOY;
                if (movementDirection.SqrMagnitude > FP._1)
                {
                    movementDirection = movementDirection.Normalized;
                }
                
                // var newPosition = UpdateMovement(f, ref filter, input, config);
                // filter.Transform->Position += newPosition;
            }

            if (!status->IsDead)
            {
                filter.KCC->Move(f, entity, movementDirection);
                UpdateRotate(f, ref filter, movementDirection);
            }

        }
        private void UpdateRotate(Frame frame, ref Filter filter, FPVector3 velocity)
        {
            if (velocity.Normalized.XZ.SqrMagnitude == FP._0) return;
            var dir = velocity.Normalized;
            
            
            var lookDir = FPQuaternion.LookRotation(dir);
            var t = FPMath.Clamp01(FP.FromFloat_UNSAFE(11f) * frame.DeltaTime * FP.FromFloat_UNSAFE(1.2f));
            filter.Transform->Rotation = FPQuaternion.Lerp(filter.Transform->Rotation, lookDir, t);
        }
        public bool OnCharacterCollision3D(FrameBase f, EntityRef character, Hit3D hit)
        {
            return true;
        }

        public void OnCharacterTrigger3D(FrameBase f, EntityRef character, Hit3D hit)
        {

        }
    }
}

