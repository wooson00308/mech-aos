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

    private void Awake()
    {
        _weaponFxs = transform.GetComponentsInChildren<ParticleSystem>().ToList();
    }

    public void SetIgnoreCollision(LayerMask mask)
    {
        foreach (var fx in _weaponFxs)
        {
            var collision = fx.collision;
            collision.collidesWith = ~mask;
        }
    }
    public void OnAttack()
    {
        // if (WeaponFx.isPlaying) return;
        foreach (var fx in _weaponFxs)
        {
            fx.Play();
        }

        AudioManager.Instance.PlaySfx(attackSound);
        OnReadyAttack(false);
    }

    public void OnReadyAttack(bool isReady)
    {
        if (EmptyModel == null) return;

        Model.enabled = isReady;
        EmptyModel.enabled = !isReady;

        if(isReady) AudioManager.Instance.PlaySfx(readySound);
    }
}
