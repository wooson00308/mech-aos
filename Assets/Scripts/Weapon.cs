using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<ParticleSystem> WeaponFxs;
    public ParticleSystem WeaponFx; 
    public void OnAttack()
    {
        //if (WeaponFx.isPlaying) return;
        foreach(var fx in WeaponFxs)
        {
            fx.Play();
        }
    }
}
