using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private List<ParticleSystem> _weaponFxs;
    public SkinnedMeshRenderer Model;
    public SkinnedMeshRenderer EmptyModel;

    public float AttackCooltime;
    public float AttackInitDelay;

    public AudioClip attackSound;
    public AudioClip readySound;

    public SkillData skillData;

    private void Awake()
    {
        _weaponFxs = transform.GetComponentsInChildren<ParticleSystem>().ToList();
    }

    public void OnAttack(Mech mech)
    {
        // if (WeaponFx.isPlaying) return;
        foreach (var fx in _weaponFxs)
        {
            fx.Play();
        }

        AudioManager.Instance.PlaySfx(attackSound);
        OnReadyAttack(mech, false);
        skillData?.OnSkillAttack(mech);
    }

    public void OnReadyAttack(Mech mech, bool isReady)
    {
        if (EmptyModel == null) return;

        Model.enabled = isReady;
        EmptyModel.enabled = !isReady;

        skillData?.OnSkillReady(mech, isReady);

        if(isReady) AudioManager.Instance.PlaySfx(readySound);
    }
}
