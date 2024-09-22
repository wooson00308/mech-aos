using System;

namespace Quantum
{
    using Photon.Deterministic;
    /// <summary>
    /// Normal bullet behavior
    /// 
    /// Deals damage on a robot.
    /// </summary>
    [System.Serializable]
    public class BulletDataCommon : BulletData
    {
        public override unsafe void BulletAction(Frame frame, EntityRef bullet, EntityRef target, EHitTargetType hitTargetTyp)
        {
            var fields = frame.Unsafe.GetPointer<BulletFields>(bullet);
            var status = frame.Unsafe.GetPointer<Status>(fields->Source);
            if (target != EntityRef.None)
            {
                switch (hitTargetTyp)
                {
                    case EHitTargetType.None:
                        break;
                    case EHitTargetType.Mechanic:
                        frame.Signals.OnMechanicHit(bullet, target, Damage * (1 + (status->Level - 1) * FP._0_10));
                        break;
                    case EHitTargetType.Nexus:
                        frame.Signals.OnNexusHit(bullet, target, Damage * (1 + (status->Level - 1) * FP._0_10));
                        break;
                }

            }
            FPVector3 position = frame.Get<Transform3D>(bullet).Position;
            frame.Events.OnBulletDestroyed(bullet.GetHashCode(), fields->Source, position, fields->Direction, fields->BulletData);
            frame.Destroy(bullet);
        }
    }
}