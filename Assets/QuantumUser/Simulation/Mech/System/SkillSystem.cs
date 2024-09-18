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
            public PlayableMechanic* Mechanic;
            public PlayerLink* PlayerLink;
            public Status* Status;
            public SkillInventory* SkillInventory;
            public WeaponInventory* WeaponInventory;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            var entity = filter.Entity;
            var playerLink = filter.PlayerLink;
            var status = filter.Status;
            var skillInventory = filter.SkillInventory;
            var weaponInventory = filter.WeaponInventory;
            var mechanic = filter.Mechanic;
            if (status->IsDead)
            {
                return;
            }
            
            var skills = frame.ResolveList(skillInventory->Skills);
            var weapons = frame.ResolveList(weaponInventory->Weapons);
            var currentWeapons = weapons[weaponInventory->CurrentWeaponIndex];
            
            OnInput(frame, mechanic, playerLink->PlayerRef, skills, currentWeapons);
            
            for (int i = 0; i < skills.Count; i++)
            {
                var skill = skills.GetPointer(i);
                var skillData = frame.FindAsset(skill->SkillData);
                StatusApply(frame, entity, skill, skillData);
            }
            
            var returnSkillData = frame.FindAsset(mechanic->ReturnSkill.SkillData);
            StatusApply(frame, entity, &mechanic->ReturnSkill, returnSkillData);


        }

        public void StatusApply(Frame frame, EntityRef entity, Skill* skill, SkillData skillData)
        {
            switch (skill->Status)
            {
                case SkillStatus.Casting:
                    skill->RemainingCastingTime -= frame.DeltaTime;
                    Debug.Log(skill->RemainingCastingTime);
                    if (skill->RemainingCastingTime <= FP._0)
                    {
                        StartSkill(frame, entity, skill);
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
        private void OnInput(Frame frame, PlayableMechanic* mechanic ,PlayerRef playerRef, QList<Skill> skills, Weapon weapon)
        {
            var input = frame.GetPlayerInput(playerRef);
            var weaponData = frame.FindAsset<PrimaryWeaponData>(weapon.WeaponData.Id);

            if (input->FirstSkill.WasPressed)
            {
                ActionSkill(weaponData, skills, 0);
            }
            if (input->SecondSkill.WasPressed)
            {
                ActionSkill(weaponData, skills, 1);
            }
            if (input->ThirdSkill.WasPressed)
            {
                ActionSkill(weaponData, skills, 2);
            }

            if (input->Return.WasPressed)
            {
                Debug.Log("리턴 실행");
                var skill = &mechanic->ReturnSkill;
                if (skill->Status != SkillStatus.Ready) return;
                skill->Status = SkillStatus.Casting;
                
                Debug.Log(skill->Status);
            }
        }

        private void ActionSkill(PrimaryWeaponData data, QList<Skill> skills, int index)
        {
            if (data.Skills.Count <= index) return;
            if (skills.Count <= data.Skills[index]) return;  
            var skill = skills.GetPointer(data.Skills[index]);
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
            Debug.Log(data);
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