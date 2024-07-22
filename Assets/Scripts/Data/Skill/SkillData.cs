using UnityEngine;

public abstract class SkillData : ScriptableObject
{
    public int id;
    public string skillName;
    public string discription;

    public abstract void OnSkillReady(Mech mech, bool isReady = true);
    public abstract void OnSkillAttack(Mech mech);
}
