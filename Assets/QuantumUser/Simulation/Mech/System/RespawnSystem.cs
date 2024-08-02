using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    using Photon.Deterministic;
    [Preserve]
    public unsafe class RespawnSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            foreach (var (mechanic, mechanicStatus) in f.Unsafe.GetComponentBlockIterator<Status>())
            {
                if (mechanicStatus->IsDead)
                {
                    mechanicStatus->RespawnTimer -= f.DeltaTime;
                    if (mechanicStatus->RespawnTimer <= FP._0)
                    {
                        RespawnHelper.RespawnMechanic(f, mechanic);
                        f.Signals.OnActiveAbilityStopped(mechanic);
                        f.Signals.OnCooldownsReset(mechanic);
                    }
                }
            }
        }
    }
}
