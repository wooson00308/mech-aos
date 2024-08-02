namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine;
    public abstract class SkillData : AssetObject
    {
        public FP CastingTime;
        public FP CoolTime;
        public virtual unsafe void Action(Frame frame, EntityRef mechanic)
        {
            
        }
    }
}