namespace Quantum
{
    public partial struct AbilityInventory
    {
        public bool HasActiveAbility => ActiveAbilityInfo.ActiveAbilityIndex >= 0;
        
        public ref Ability GetAbility(AbilityType abilityType)
        {
            return ref Abilities[(int)abilityType];
        }

        public bool TryGetActiveAbility(out Ability ability)
        {
            if (!HasActiveAbility)
            {
                ability = default;
                return false;
            }

            ability = Abilities[ActiveAbilityInfo.ActiveAbilityIndex];
            return true;
        }
    }
}