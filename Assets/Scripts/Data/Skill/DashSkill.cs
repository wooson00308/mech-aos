using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash Skill", menuName = "Data/Create Dash Skill")]
public class DashSkill : SkillData
{
    public override void OnSkillAttack(Mech mech)
    {
        mech.Move.OnDash();
    }

    public override void OnSkillReady(Mech mech, bool isReady)
    {
        
    }
}
