using Photon.Deterministic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class NexusSystem : SystemSignalsOnly, ISignalOnNexusHit,
        ISignalOnTriggerEnter3D, ISignalOnTriggerExit3D
    {
        public void OnNexusHit(Frame frame, EntityRef bullet, EntityRef nexus, FP damage)
        {
            var shooter = bullet;
            if(frame.TryGet<TrapFields>(bullet,out var trapFields))
                shooter = trapFields.Source;
            else if (frame.TryGet<BulletFields>(bullet,out var bulletFields))
                shooter = bulletFields.Source;
            
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

        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info)
        {
            // entity가 메카일수도 넥서스 충돌 범위일수도 
            if (f.Has<NexusIdentifier>(info.Entity) &&
                f.Has<PlayableMechanic>(info.Other))
            {
                f.Events.OnEnableFix(info.Other, info.Entity, true);
                f.Signals.OnEnableFix(info.Other, info.Entity, true);

            }
            else if (f.Has<NexusIdentifier>(info.Other) &&
                     f.Has<PlayableMechanic>(info.Entity))
            {
                f.Events.OnEnableFix(info.Entity, info.Other, true);
                f.Signals.OnEnableFix(info.Entity, info.Other, true);
            }
        }

        public void OnTriggerExit3D(Frame f, ExitInfo3D info)
        {

            if (f.Has<NexusIdentifier>(info.Entity) &&
                f.Has<PlayableMechanic>(info.Other))
            {
                f.Events.OnEnableFix(info.Other, info.Entity, false);
                f.Signals.OnEnableFix(info.Other, info.Entity, false);
            }
            else if (f.Has<NexusIdentifier>(info.Other) &&
                     f.Has<PlayableMechanic>(info.Entity))
            {
                f.Events.OnEnableFix(info.Entity, info.Other, false);
                f.Signals.OnEnableFix(info.Entity, info.Other, false);
            }
        }

    }
}