
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class AbilitySystem : SystemMainThreadFilter<AbilitySystem.Filter>, ISignalOnActiveAbilityStopped, ISignalOnComponentAdded<AbilityInventory>
    {
        public struct Filter
        {
            public EntityRef EntityRef;
            public PlayerLink* PlayerStatus;
            public AbilityInventory* AbilityInventory;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            for (int i = 0; i < filter.AbilityInventory->Abilities.Length; i++)
            {
                ref Ability ability = ref filter.AbilityInventory->Abilities[i];
                AbilityData abilityData = frame.FindAsset<AbilityData>(ability.AbilityData.Id);

                abilityData.UpdateAbility(frame, filter.EntityRef, ref ability);
                abilityData.TryActivateAbility(frame, filter.EntityRef, filter.PlayerStatus, ref ability);
            }
        }

        public void OnActiveAbilityStopped(Frame frame, EntityRef playerEntityRef)
        {
            AbilityInventory* abilityInventory = frame.Unsafe.GetPointer<AbilityInventory>(playerEntityRef);

            if (!abilityInventory->HasActiveAbility)
            {
                return;
            }

            for (int i = 0; i < abilityInventory->Abilities.Length; i++)
            {
                Ability ability = abilityInventory->Abilities[i];

                if (ability.IsDelayedOrActive)
                {
                    ability.StopAbility(frame, playerEntityRef);
                    break;
                }
            }
        }

        public void OnAdded(Frame frame, EntityRef entity, AbilityInventory* abilityInventory)
        {
            abilityInventory->ActiveAbilityInfo.ActiveAbilityIndex = -1;

            for (int i = 0; i < abilityInventory->Abilities.Length; i++)
            {
                abilityInventory->Abilities[i].AbilityType = (AbilityType)i;
            }
        }
    }
}