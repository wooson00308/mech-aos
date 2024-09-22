using Photon.Deterministic;

namespace Quantum
{
    public class MeleeAttackData : AssetObject
    {
        public Shape3DConfig ShapeConfig;
        public FP Damage;
        
        public virtual unsafe void Action(Frame frame, EntityRef attacker, EntityRef target, EHitTargetType hitTargetType)
        {
            if (target == EntityRef.None || !frame.Has<Status>(target))
            {
                return;
            }
            frame.Signals.OnMechanicHit(attacker, target, Damage);
        }
    }
}