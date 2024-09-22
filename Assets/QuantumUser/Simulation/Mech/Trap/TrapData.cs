using Photon.Deterministic;

namespace Quantum
{
    public class TrapData: AssetObject
    {
        public Shape3DConfig ShapeConfig;
        public FP Damage;
        public FP Latency;
        
        public virtual unsafe void Action(Frame frame, EntityRef trap, EntityRef target)
        {
            if (target == EntityRef.None || !frame.Has<Status>(target))
            {
                return;
            }
            frame.Signals.OnMechanicHit(trap, target, Damage);
            
            var fields = frame.Get<TrapFields>(trap);
            var position = frame.Get<Transform3D>(trap).Position;
            frame.Events.OnTrapDestroyed(trap.GetHashCode(), fields.Source, position, fields.TrapData);
            frame.Destroy(trap);
        }
    }
}