using Photon.Deterministic;

namespace Quantum
{
    public unsafe partial struct Skill
    {
        public void OnReset()
        {
            RemainingCastingTime = FP._0;
            RemainingCoolTime = FP._0;
            Status = SkillStatus.Ready;
        }
    }
}