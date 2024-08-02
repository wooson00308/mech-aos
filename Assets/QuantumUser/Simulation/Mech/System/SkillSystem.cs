using System;
using UnityEngine.Scripting;
using Photon.Deterministic;
using Quantum.Collections;
using UnityEngine;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class SkillSystem: SystemMainThreadFilter<SkillSystem.Filter>, ISignalOnMechanicRespawn, ISignalOnGameEnded, ISignalOnCooldownsReset
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* PlayerLink;
            public Status* Status;
            public SkillInventory* SkillInventory;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            var mechanic = filter.Entity;
            var playerLink = filter.PlayerLink;
            var status = filter.Status;
            var skillInventory = filter.SkillInventory;
            if (status->IsDead)
            {
                return;
            }
            
            var skills = frame.ResolveList(skillInventory->Skills);
            
            OnInput(frame, playerLink->PlayerRef, skills);
            
            for (int i = 0; i < skills.Count; i++)
            {
                var skill = skills.GetPointer(i);
                var skillData = frame.FindAsset(skill->SkillData);
                switch (skill->Status)
                {
                    case SkillStatus.Casting:
                        skill->RemainingCastingTime -= frame.DeltaTime;
                        if (skill->RemainingCastingTime <= FP._0)
                        {
                            StartSkill(frame, mechanic, skill);
                            skill->RemainingCastingTime = skillData.CastingTime;
                            skill->Status = SkillStatus.CoolTime;
                        }
                        break;
                    case SkillStatus.CoolTime:
                        skill->RemainingCoolTime -= frame.DeltaTime;
                        if (skill->RemainingCoolTime <= FP._0)
                        {
                            skill->RemainingCoolTime = skillData.CoolTime;
                            skill->Status = SkillStatus.Ready;
                        }
                        break;
                }
            }
                

        }
        private void OnInput(Frame frame, PlayerRef playerRef, QList<Skill> skills)
        {
            var input = frame.GetPlayerInput(playerRef);
            if (input->FirstSkill.WasPressed)
            {
                ActionSkill(skills, 0);
            }
            if (input->SecondSkill.WasPressed)
            {
                ActionSkill(skills, 1);
            }
            if (input->ThirdSkill.WasPressed)
            {
                ActionSkill(skills, 2);
            }
        }
        private void ActionSkill(QList<Skill> skills, int index)
        {
            if (skills.Count < index) return;  
            var skill = skills.GetPointer(index);
            if (skill->Status != SkillStatus.Ready) return;
            skill->Status = SkillStatus.Casting;
        }
        
        public void OnMechanicRespawn(Frame frame, EntityRef robot)
        {
            // throw new System.NotImplementedException();
        }
        public unsafe void OnGameEnded(Frame frame, GameController* gameController)
        {
            frame.SystemDisable<SkillSystem>();
        }
        private void StartSkill(Frame frame, EntityRef mechanic, Skill* skill)
        {
            var data = frame.FindAsset(skill->SkillData);
            data.Action(frame, mechanic);

        }
        public void OnCooldownsReset(Frame frame, EntityRef playerEntityRef)
        {
            SkillInventory* skillInventory = frame.Unsafe.GetPointer<SkillInventory>(playerEntityRef);
            var skills = frame.ResolveList(skillInventory->Skills);
            for (int i = 0; i < skills.Count; i++)
            {
                skills[i].OnReset();
            }
        }
    }
}