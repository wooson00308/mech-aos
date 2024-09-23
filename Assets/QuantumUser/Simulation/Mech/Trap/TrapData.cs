using Photon.Deterministic;
using UnityEngine;

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
            var fields = frame.Unsafe.GetPointer<TrapFields>(trap);
            var transform3D = frame.Unsafe.GetPointer<Transform3D>(trap);
            var status = frame.Unsafe.GetPointer<Status>(fields->Source);
            
            frame.Signals.OnMechanicHit(trap, target, Damage * (1 + (status->Level - 1) * FP._0_10));
            frame.Events.OnTrapDestroyed(trap.GetHashCode(), fields->Source, transform3D->Position, fields->TrapData);
            frame.Destroy(trap);
        }
    }
}