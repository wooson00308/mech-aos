using System;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    [Serializable]
    public unsafe partial class RecallAbilityData : AbilityData
    {
        private bool isStart;
        private FPVector3 teleportPosition;
        public override Ability.AbilityState UpdateAbility(Frame frame, EntityRef entityRef, ref Ability ability)
        {
            Ability.AbilityState abilityState = base.UpdateAbility(frame, entityRef, ref ability);
            if (abilityState.IsActive)
            {
                var abilityInventory = frame.Unsafe.GetPointer<AbilityInventory>(entityRef);
                if (abilityInventory->HasActiveAbility && !isStart)
                {
                    var transform3D = frame.Unsafe.GetPointer<Transform3D>(entityRef);
                    transform3D->Position = teleportPosition ;
                    transform3D->Teleport(frame, teleportPosition);

                    StopAbility(frame, entityRef, ref ability);
                    return abilityState;
                }

                if (!abilityState.IsActiveEndTick) return abilityState;
                
                var playableMechanic = frame.Unsafe.GetPointer<PlayableMechanic>(entityRef);
                
                foreach (var pair in frame.Unsafe.GetComponentBlockIterator<SpawnIdentifier>())
                {
                    if(pair.Component->Team != playableMechanic->Team) continue;
                        
                    var spawnTransform = frame.Get<Transform3D>(pair.Entity);
                    teleportPosition = spawnTransform.Position;
                                            
                    Transform3D* transform = frame.Unsafe.GetPointer<Transform3D>(entityRef);
                    transform->Position = teleportPosition ;
                    transform->Teleport(frame, teleportPosition);
                    break;
                }

            }
            

            return abilityState;
        }
        
        public override unsafe bool TryActivateAbility(Frame frame, EntityRef entityRef, PlayerLink* playerStatus, ref Ability ability)
        {
            bool activated = base.TryActivateAbility(frame, entityRef, playerStatus, ref ability);

            if (activated)
            {
                isStart = true;
            }

            return activated;
        }
        public override void StopAbility(Frame frame, EntityRef entityRef, ref Ability ability)
        {
            base.StopAbility(frame, entityRef, ref ability);
            isStart = false;
        }
    }
}