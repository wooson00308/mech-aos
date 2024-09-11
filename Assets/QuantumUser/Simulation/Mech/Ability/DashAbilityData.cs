using System;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    [Serializable]
    public unsafe partial class DashAbilityData : AbilityData
    {
        public FP DashDistance = 5;
        public FPAnimationCurve DashMovementCurve;

        public override Ability.AbilityState UpdateAbility(Frame frame, EntityRef entityRef, ref Ability ability)
        {
            FP lastNormalizedTime = FP._0;
            if (ability.IsActive)
            {
                lastNormalizedTime = ability.DurationTimer.NormalizedTime;
            }

            Ability.AbilityState abilityState = base.UpdateAbility(frame, entityRef, ref ability);
            
            if (abilityState.IsActive)
            {
                AbilityInventory* abilityInventory = frame.Unsafe.GetPointer<AbilityInventory>(entityRef);
                Transform3D* transform = frame.Unsafe.GetPointer<Transform3D>(entityRef);
                CharacterController3D* kcc = frame.Unsafe.GetPointer<CharacterController3D>(entityRef);

                FP lastNormalizedPosition = DashMovementCurve.Evaluate(lastNormalizedTime);
                FPVector3 lastRelativePosition = abilityInventory->ActiveAbilityInfo.CastDirection * DashDistance * lastNormalizedPosition;

                FP newNormalizedTime = ability.DurationTimer.NormalizedTime;
                FP newNormalizedPosition = DashMovementCurve.Evaluate(newNormalizedTime);
                FPVector3 newRelativePosition = abilityInventory->ActiveAbilityInfo.CastDirection * DashDistance * newNormalizedPosition;

                var direction = newRelativePosition - lastRelativePosition;
                var hits = frame.Physics3D.RaycastAll(transform->Position, direction.Normalized, direction.Magnitude);
                if(hits.Count <= 0) transform->Position += newRelativePosition - lastRelativePosition;

                if (abilityState.IsActiveEndTick)
                {
                    kcc->Velocity = abilityInventory->ActiveAbilityInfo.CastDirection * kcc->MaxSpeed;
                }
            }

            return abilityState;
        }

        public override unsafe bool TryActivateAbility(Frame frame, EntityRef entityRef, PlayerLink* playerStatus, ref Ability ability)
        {
            bool activated = base.TryActivateAbility(frame, entityRef, playerStatus, ref ability);

            if (activated)
            {
                frame.Events.OnMechanicDashed(entityRef);
            }

            return activated;
        }
    }
}