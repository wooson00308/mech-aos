using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    public unsafe partial class AbilityData : AssetObject
    {
        public FP InputBuffer = FP._0_10 + FP._0_05;
        public FP Delay = FP._0_10 + FP._0_05;
        public FP Duration = FP._0_25;
        public EAbilityEndCondition EndCondition;
        public bool KeepVelocity = false;
        public virtual Ability.AbilityState UpdateAbility(Frame frame, EntityRef entityRef, ref Ability ability)
        {
            return ability.Update(frame, entityRef);
        }

        public virtual void UpdateInput(Frame frame, ref Ability ability)
        {
            ability.BufferInput(frame);
        }

        public virtual bool TryActivateAbility(Frame frame, EntityRef entityRef, PlayerLink* playerStatus, ref Ability ability)
        {
            if (ability.HasBufferedInput)
            {
                if (ability.TryActivateAbility(frame, entityRef, playerStatus->PlayerRef))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void StopAbility(Frame frame, EntityRef entityRef, ref Ability ability)
        {
            ability.StopAbility(frame, entityRef);
        }
    }
}