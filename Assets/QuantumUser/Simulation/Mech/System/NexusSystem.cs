using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class NexusSystem : SystemSignalsOnly, ISignalOnNexusHit
    {
        public void OnNexusHit(Frame frame, EntityRef bullet, EntityRef nexus, FP damage)
        {
            var isSuccess = frame.TryGet<BulletFields>(bullet, out var fields);
            
            // TODO 조심할것
            var shooter = isSuccess ? fields.Source : bullet;
            
            TakeDamage(frame, shooter, nexus, damage);
        }
        private void TakeDamage(Frame frame, EntityRef attacker, EntityRef nexus, FP damage)
        {
            var nexusStatus = frame.Unsafe.GetPointer<Nexus>(nexus);

            if (damage < FP._1)
            {
                return;
            }
            
            nexusStatus->CurrentHealth -= damage;
            frame.Events.OnNexusTakeDamage(nexus, damage, attacker);
            // frame.Events.OnRobotBlink(target);

            if (nexusStatus->CurrentHealth <= 0)
            {
                DestroyNexus(frame, attacker, nexus, nexusStatus);
            }
        }
        
        private void DestroyNexus(Frame frame, EntityRef killer, EntityRef nexus, Nexus* nexusStatus)
        {
            nexusStatus->CurrentHealth = FP._0;
            nexusStatus->IsDestroy = true;
            var collider3D = frame.Unsafe.GetPointer<PhysicsCollider3D>(nexus);
            collider3D->Enabled = false;
            
            
            frame.Signals.OnNexusDestroy(nexus, killer); // 현재 안씀
            frame.Events.OnNexusDestroy(nexus, killer);

            frame.Signals.OnTeamDefeat(nexusStatus->Team);
            
        }



    }
}