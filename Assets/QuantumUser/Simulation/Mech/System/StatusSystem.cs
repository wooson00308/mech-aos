using Photon.Deterministic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class StatusSystem  : SystemMainThreadFilter<StatusSystem.Filter>, ISignalOnMechanicRespawn, ISignalOnMechanicHit, ISignalOnMechanicSkillHit
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public Status* Status;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            var status = filter.Status;
            
            var statusData = frame.FindAsset<StatusData>(status->StatusData.Id);
            status->RegenTimer -= frame.DeltaTime;
            if (status->RegenTimer < 0)
            {
                status->CurrentHealth += frame.DeltaTime * statusData.RegenRate;
                status->CurrentHealth = FPMath.Clamp(status->CurrentHealth, status->CurrentHealth, statusData.MaxHealth);
            }
            
            if (status->InvincibleTimer > FP._0)
            {
                status->InvincibleTimer -= frame.DeltaTime;
            }
        }

        public void OnMechanicRespawn(Frame frame, EntityRef robot)
        {
            Status* status = frame.Unsafe.GetPointer<Status>(robot);
            StatusData statusData = frame.FindAsset<StatusData>(status->StatusData.Id);

            status->IsDead = false;
            status->CurrentHealth = statusData.MaxHealth;
            status->InvincibleTimer = statusData.InvincibleTime;
        }

        public void OnMechanicHit(Frame frame, EntityRef bullet, EntityRef robot, FP damage)
        {
            // EntityRef shooter = frame.Get<BulletFields>(bullet).Source;
            TakeDamage(frame, bullet, robot, damage);
        }

        public void OnMechanicSkillHit(Frame frame, EntityRef skill, EntityRef robot)
        {
            // SkillFields skillFields = frame.Get<SkillFields>(skillRef);
            // SkillData skillData = frame.FindAsset<SkillData>(skillFields.SkillData.Id);
            // EntityRef caster = skillFields.Source;
            // TakeDamage(frame, caster, robotRef, skillData.Damage);
        }
        
        private static void TakeDamage(Frame frame, EntityRef attacker, EntityRef target, FP damage)
        {
            Status* mechanicStatus = frame.Unsafe.GetPointer<Status>(target);

            if (mechanicStatus->InvincibleTimer > FP._0 || damage < FP._1)
            {
                return;
            }

            StatusData statusData = frame.FindAsset<StatusData>(mechanicStatus->StatusData.Id);

            mechanicStatus->RegenTimer = statusData.TimeUntilRegen;
            mechanicStatus->CurrentHealth -= damage;
            frame.Events.OnMechanicTakeDamage(target, damage, attacker);
            // frame.Events.OnRobotBlink(target);

            if (mechanicStatus->CurrentHealth <= 0)
            {
                KillMechanic(frame, attacker, target, mechanicStatus, statusData.RespawnTime);
            }
        }
        
        private static void KillMechanic(Frame frame, EntityRef killer, EntityRef mechanic, Status* mechanicStatus, FP respawnTime)
        {
            CharacterController3D* characterController = frame.Unsafe.GetPointer<CharacterController3D>(mechanic);
            PhysicsCollider3D* collider = frame.Unsafe.GetPointer<PhysicsCollider3D>(mechanic);
            
            mechanicStatus->CurrentHealth = FP._0;
            mechanicStatus->IsDead = true;
            mechanicStatus->RespawnTimer = respawnTime;
            characterController->Velocity = FPVector3.Zero;
            collider->IsTrigger = true;
            
            frame.Signals.OnMechanicDeath(mechanic, killer);
            frame.Events.OnMechanicDeath(mechanic, killer);

        }
    }
}