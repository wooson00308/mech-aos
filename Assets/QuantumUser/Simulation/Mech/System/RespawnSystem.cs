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
                if (mechanicStatus->IsDead && IsRespawnTimerAllowed(f, f.Get<PlayableMechanic>(mechanic).Team))
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
        private bool IsRespawnTimerAllowed(Frame f,Team team)
        {
            foreach (var pair in f.Unsafe.GetComponentBlockIterator<Nexus>())
            {
                if(pair.Component->Team != team) continue;
                return !pair.Component->IsDestroy;
            }

            return false;
        }
    }
}
