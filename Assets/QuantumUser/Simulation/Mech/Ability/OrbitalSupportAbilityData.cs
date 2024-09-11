using System;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    [Serializable]
    public unsafe partial class OrbitalSupportAbilityData : AbilityData
    {
        private Camera _camera;
        private bool isStart;
        private FPVector3 teleportPosition;
        public override Ability.AbilityState UpdateAbility(Frame frame, EntityRef entityRef, ref Ability ability)
        {
            Ability.AbilityState abilityState = base.UpdateAbility(frame, entityRef, ref ability);
            if (abilityState.IsActive)
            {
                // var abilityInventory = frame.Unsafe.GetPointer<AbilityInventory>(entityRef);
                // if (abilityInventory->HasActiveAbility && !isStart)
                // {
                //     var transform3D = frame.Unsafe.GetPointer<Transform3D>(entityRef);
                //     transform3D->Position = teleportPosition ;
                //     transform3D->Teleport(frame, teleportPosition);
                //
                //     StopAbility(frame, entityRef, ref ability);
                //     return abilityState;
                // }
                
                if(_camera == null) _camera = Camera.main;

                var _playerLink = frame.Unsafe.GetPointer<PlayerLink>(entityRef);
                Input* input = frame.GetPlayerInput(_playerLink->PlayerRef);
                
                if (input->MouseLeftButton && !abilityState.IsActiveEndTick)
                {
                    if (RaycastFromScreenPoint(frame, entityRef, input->MousePosition))
                    {
                        abilityState.IsActiveEndTick = true;
                        ability.StopAbility(frame, entityRef);
                        frame.Signals.OnMechanicTeleport(entityRef,teleportPosition);
                        // var transform3D = frame.Unsafe.GetPointer<Transform3D>(entityRef);
                        // transform3D->Position = teleportPosition ;
                        // transform3D->Teleport(frame, teleportPosition);
                        frame.Events.OnMechanicOrbitalSupportEnd(entityRef);
                        isStart = false;
                        
                    }             
                }
            }
            

            return abilityState;
        }

        bool RaycastFromScreenPoint(Frame frame, EntityRef entityRef, FPVector2 screenPosition)
        {
            Ray ray = _camera.ScreenPointToRay(new Vector3(screenPosition.X.AsFloat, screenPosition.Y.AsFloat , 0));
            var origin = new FPVector3(FP.FromFloat_UNSAFE(ray.origin.x),FP.FromFloat_UNSAFE(ray.origin.y),FP.FromFloat_UNSAFE(ray.origin.z));
            var direction = new FPVector3(FP.FromFloat_UNSAFE(ray.direction.x),FP.FromFloat_UNSAFE(ray.direction.y),FP.FromFloat_UNSAFE(ray.direction.z));

            var hit3D = frame.Physics3D.Raycast(origin, direction, 1000);

            if (hit3D is not { IsStatic: true }) return false;

            var index = hit3D.Value.StaticColliderIndex;
            var isOrbitalSupport = frame.FindAsset<OrbitalSupportEnvironmentData>(frame.Map.StaticColliders3D[index].StaticData.Asset);
            
            if (isOrbitalSupport == null) return false;
            teleportPosition = hit3D.Value.Point + (FPVector3.Up / 2);
            
            return true;
        }
        
        public override unsafe bool TryActivateAbility(Frame frame, EntityRef entityRef, PlayerLink* playerStatus, ref Ability ability)
        {
            bool activated = base.TryActivateAbility(frame, entityRef, playerStatus, ref ability);

            if (activated)
            {
                frame.Events.OnMechanicOrbitalSupport(entityRef);
                isStart = true;
            }

            return activated;
        }
        
    }
}