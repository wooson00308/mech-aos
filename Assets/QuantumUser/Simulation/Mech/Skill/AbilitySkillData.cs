using UnityEngine;

namespace Quantum
{
    // 어빌리티는 현재 한번에 한개밖에 안됨
    public unsafe class AbilitySkillData : SkillData
    {
        public AbilityType abilityType;
        private int _abilityIndex = -1;
        
        public override void Action(Frame frame, EntityRef mechanic)
        {
            if (!frame.Unsafe.TryGetPointer(mechanic, out AbilityInventory* abilityInventory)) return;
            if (_abilityIndex == -1) _abilityIndex = GetAbility(abilityInventory->Abilities);
            if (_abilityIndex == -2) return;

            Debug.Log("어빌리티 스킬 실행!");
            ref var ability = ref abilityInventory->Abilities[_abilityIndex];
            
            var abilityData = frame.FindAsset<AbilityData>(ability.AbilityData.Id);
            
            abilityData.UpdateInput(frame, ref ability);
        }

        private int GetAbility(FixedArray<Ability> array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                if (array[i].AbilityType != abilityType) continue;
                return i;
            }
            return -2;
        }
    }
}