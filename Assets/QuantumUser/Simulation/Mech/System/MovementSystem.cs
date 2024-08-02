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
        // private FPVector3 UpdateMovement(Frame frame, ref Filter filter, Input* input, MechGameConfig config)
        // {
        //     var mech = filter.Entity;
        //     var transform = filter.Transform;
        //     var kcc = filter.KCC;
        //     
        //     Assert.Check(frame.Has<Transform3D>(mech));
        //     
        //     FPVector3 direction = input->Movement.XOY;
        //     
        //     
        //     
        //     var velocity = direction.Normalized * config.MechMovementSpeed;
        //     // var layer = frame.Layers.GetLayerMask("Environment");
        //     
        //     var movementPack = CharacterController3D.ComputeRawMovement(frame, mech, transform, kcc, velocity, this);
        //     // CheckVisualGrounded(frame, robotMovement, movementPack, mech);
        //     
        //     var kccConfig = frame.FindAsset<CharacterController3DConfig>(kcc->Config.Id);
        //     ComputeRawSteer(kcc, ref movementPack, frame.DeltaTime, kccConfig);
        //     
        //     
        //     var movement = kcc->Velocity * frame.DeltaTime;
        //     
        //     if (movementPack.Penetration > FP.EN3)
        //     {
        //         if (movementPack.Penetration > kccConfig.MaxPenetration)
        //         {
        //             movement += movementPack.Correction;
        //         }
        //         else
        //         {
        //             movement += movementPack.Correction * frame.DeltaTime * kccConfig.Acceleration;
        //         }
        //     }
        //     
        //     return movement;
        // }
        private void UpdateRotate(Frame frame, ref Filter filter, FPVector3 velocity)
        {
            if (velocity.Normalized.XZ.SqrMagnitude == FP._0) return;
            var dir = velocity.Normalized;
            
            
            var lookDir = FPQuaternion.LookRotation(dir);
            var t = FPMath.Clamp01(FP.FromFloat_UNSAFE(11f) * frame.DeltaTime * FP.FromFloat_UNSAFE(1.2f));
            filter.Transform->Rotation = FPQuaternion.Lerp(filter.Transform->Rotation, lookDir, t);
        }
        // static void ComputeRawSteer(CharacterController3D* kcc, ref CharacterController3DMovement movementPack, FP deltaTime, CharacterController3DConfig config)
        // {
        //     kcc->Grounded = movementPack.Grounded;
        //
        //     FP maxYSpeed = FP._100;
        //     FP minYSpeed = -FP._100;
        //     
        //     FP targetSpeed = kcc->MaxSpeed;
        //     FP targetSpeedSq = targetSpeed * targetSpeed;
        //     
        //     switch (movementPack.Type)
        //     {
        //         case CharacterMovementType.FreeFall:
        //             kcc->Velocity.Y -= config.Gravity.Magnitude * deltaTime;
        //             if (config.AirControl == false || movementPack.Tangent == default(FPVector3))
        //             {
        //                 kcc->Velocity.X = FPMath.Lerp(kcc->Velocity.X, FP._0, deltaTime * config.Braking);
        //                 kcc->Velocity.Z = FPMath.Lerp(kcc->Velocity.Z, FP._0, deltaTime * config.Braking);
        //             }
        //             else
        //             {
        //                 kcc->Velocity += movementPack.Tangent * config.Acceleration * deltaTime;
        //             }
        //             break;
        //         case CharacterMovementType.Horizontal:
        //             kcc->Velocity += movementPack.Tangent * config.Acceleration * deltaTime;
        //             var tangentSpeed = FPVector3.Dot(kcc->Velocity, movementPack.Tangent);
        //             var tangentVel = movementPack.Tangent * kcc->MaxSpeed;
        //
        //             kcc->Velocity.X = FPMath.Lerp(kcc->Velocity.X, tangentVel.X, deltaTime * config.Acceleration);
        //             kcc->Velocity.Z = FPMath.Lerp(kcc->Velocity.Z, tangentVel.Z, deltaTime * config.Acceleration);
        //     
        //             // we only lerp the vertical velocity if the character is not jumping in this exact frame,
        //             // otherwise it will jump with a lower impulse
        //             if (kcc->Jumped == false) {
        //                 kcc->Velocity.Y = FPMath.Lerp(kcc->Velocity.Y, tangentVel.Y, deltaTime * config.Acceleration);
        //             }
        //
        //             // clamp tangent velocity with max speed
        //             if (tangentSpeed * tangentSpeed > targetSpeedSq) {
        //                 kcc->Velocity -= movementPack.Tangent * (tangentSpeed - kcc->MaxSpeed);
        //             }
        //             break;
        //         case CharacterMovementType.SlopeFall:
        //             kcc->Velocity += movementPack.SlopeTangent * config.Acceleration * deltaTime;
        //             minYSpeed = -config.MaxSlopeSpeed;
        //             break;
        //         case CharacterMovementType.None:
        //             var lerpFactor = deltaTime * config.Braking;
        //
        //             if (kcc->Velocity.X.RawValue != 0) {
        //                 kcc->Velocity.X = FPMath.Lerp(kcc->Velocity.X, default, lerpFactor);
        //                 if (FPMath.Abs(kcc->Velocity.X) < FP.EN1) {
        //                     kcc->Velocity.X.RawValue = 0;
        //                 }
        //             }
        //
        //             if (kcc->Velocity.Z.RawValue != 0) {
        //                 kcc->Velocity.Z = FPMath.Lerp(kcc->Velocity.Z, default, lerpFactor);
        //                 if (FPMath.Abs(kcc->Velocity.Z) < FP.EN1) {
        //                     kcc->Velocity.Z.RawValue = 0;
        //                 }
        //             }
        //
        //             // we only lerp the vertical velocity back to 0 if the character is not jumping in this exact frame,
        //             // otherwise it will jump with a lower impulse
        //             if (kcc->Velocity.Y.RawValue != 0 && kcc->Jumped == false) {
        //                 kcc->Velocity.Y = FPMath.Lerp(kcc->Velocity.Y, default, lerpFactor);
        //                 if (FPMath.Abs(kcc->Velocity.Y) < FP.EN1) {
        //                     kcc->Velocity.Y.RawValue = 0;
        //                 }
        //             }
        //
        //             minYSpeed = 0;
        //             break;
        //     }
        //
        //     // horizontal is clamped elsewhere
        //     if (movementPack.Type != CharacterMovementType.Horizontal) {
        //         var h = kcc->Velocity.XZ;
        //
        //         if (h.SqrMagnitude > targetSpeedSq) {
        //             h = h.Normalized * targetSpeed;
        //         }
        //
        //         kcc->Velocity.X = h.X;
        //         kcc->Velocity.Y = FPMath.Clamp(kcc->Velocity.Y, minYSpeed, maxYSpeed);
        //         kcc->Velocity.Z = h.Y;
        //     }
        //
        //     // reset jump state
        //     kcc->Jumped = false;
        // }
        //
        public bool OnCharacterCollision3D(FrameBase f, EntityRef character, Hit3D hit)
        {
            return true;
        }

        public void OnCharacterTrigger3D(FrameBase f, EntityRef character, Hit3D hit)
        {

        }
    }
}

