using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    public unsafe partial class AbilityData : AssetObject
    {
        public FP InputBuffer = FP._0_10 + FP._0_05;
        public FP Delay = FP._0_10 + FP._0_05;
        public FP Duration = FP._0_25;
        
        public bool KeepVelocity = false;
        public virtual Ability.AbilityState UpdateAbility(Frame frame, EntityRef entityRef, ref Ability ability)
        {
            return ability.Update(frame, entityRef);
        }

        public virtual void UpdateInput(Frame frame, ref Ability ability)
        {
            Debug.Log("어빌리티 스킬 인풋!");
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
    }
}