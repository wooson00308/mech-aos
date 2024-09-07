using System;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    [Serializable]
    public unsafe partial class OrbitalSupportAbilityData : AbilityData
    {
        public override Ability.AbilityState UpdateAbility(Frame frame, EntityRef entityRef, ref Ability ability)
        {
            Ability.AbilityState abilityState = base.UpdateAbility(frame, entityRef, ref ability);
            
            if (abilityState.IsActive)
            {
                // if(Mouse.current.leftButton.isPressed)
                // {
                //     RaycastHit hit;
                //     if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity))//레이케스트(마우스 클릭위치,레이케스트가 맞은곳,무한대기)
                //     {   if(hit.collider.tag=="땅")
                //         {
                //             movemont(hit.point);//플레이어의 위치를 레이케스트가 맞은곳으로 이동  
                //         }
                //         if(hit.collider.tag=="오브젝트")
                //         {
                //             Debug.Log("오브젝트에 맞음");
                //         } 
                //     }
                // }
            }

            return abilityState;
        }

        public override unsafe bool TryActivateAbility(Frame frame, EntityRef entityRef, PlayerLink* playerStatus, ref Ability ability)
        {
            bool activated = base.TryActivateAbility(frame, entityRef, playerStatus, ref ability);

            if (activated)
            {
                frame.Events.OnMechanicOrbitalSupport(entityRef);
            }

            return activated;
        }

        public override void StopAbility(Frame frame, EntityRef entityRef, ref Ability ability)
        {
            base.StopAbility(frame, entityRef, ref ability);
            frame.Events.OnMechanicOrbitalSupportEnd(entityRef);
        }
    }
}