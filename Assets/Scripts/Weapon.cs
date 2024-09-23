using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum.Mech
{
    public class Weapon : MonoBehaviour
    {
        private List<ParticleSystem> _weaponFxs;

        public SkinnedMeshRenderer Model;
        public SkinnedMeshRenderer EmptyModel;
        
        public AudioClip attackSound;
        public AudioClip readySound;

        public void Awake()
        {
            _weaponFxs = transform.GetComponentsInChildren<ParticleSystem>().ToList();
            foreach (var system in _weaponFxs)
            {
                system.useAutoRandomSeed = false;
                system.randomSeed = UInt32.MinValue;
            }
        }
        public void OnAttack()
        {
            // if (WeaponFx.isPlaying) return;
            foreach (var fx in _weaponFxs)
            {
                fx.Play();
            }

            //AudioManager.Instance.PlaySfx(attackSound);
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
}

