using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechHealth : MonoBehaviour
{
    private float _health;
    public Animator Animator;
    public GameObject DeathFx;

    public void Initialized(Mech mech)
    {
        DeathFx.SetActive(false);
        _health = mech.MaxHealth;
    }

    private void Hit() 
    {
        _health -= 10;

        if(_health < 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Animator.SetBool("IsDead", true);
        DeathFx.SetActive(true);
        Debug.Log("IsDeath");
    }

    private void OnParticleCollision(GameObject other)
    {
        Hit();
        Animator.SetTrigger("Hit");
    }
}
