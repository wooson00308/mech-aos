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
            if (target != EntityRef.None)
            {
                switch (hitTargetTyp)
                {
                    case EHitTargetType.None:
                        break;
                    case EHitTargetType.Mechanic:
                        frame.Signals.OnMechanicHit(bullet, target, Damage);
                        break;
                    case EHitTargetType.Nexus:
                        frame.Signals.OnNexusHit(bullet, target, Damage);
                        break;
                }

            }
            BulletFields fields = frame.Get<BulletFields>(bullet);
            FPVector3 position = frame.Get<Transform3D>(bullet).Position;
            frame.Events.OnBulletDestroyed(bullet.GetHashCode(), fields.Source, position, fields.Direction, fields.BulletData);
            frame.Destroy(bullet);
        }
    }
}