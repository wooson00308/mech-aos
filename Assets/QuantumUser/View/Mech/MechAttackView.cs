using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Quantum;
using UnityEngine;

namespace Quantum.Mech
{
    public class MechAttackView : QuantumCallbacks
    {
        private QuantumEntityView _entityView;
        public Animator Aniamtor;

        public Weapon MainWeapon;
        public Weapon SubWeapon;
        
        int _targetHandle;
        public int TargetHandle => _targetHandle;

        static int playerHandle = 0;
        public int Handle { get; private set; }
        
        private Dictionary<Weapon, bool> _weaponDelayDic = new();

        private bool MainAttackInteractable;
        private bool SubAttackInteractable;

        private void Awake()
        {
            _entityView = GetComponent<QuantumEntityView>();
            
            _weaponDelayDic.Add(MainWeapon, false);
            _weaponDelayDic.Add(SubWeapon, false);
            
            playerHandle++;
            Handle = playerHandle;

            QuantumEvent.Subscribe<EventWeaponFire>(this, WeaponFire);
        }

        public override void OnUpdateView(QuantumGame game)
        {
            Aniamtor.SetBool("IsAttack", _weaponDelayDic[MainWeapon]);
            
            MainAttackInteractable = _targetHandle != 0;
            SubAttackInteractable = _targetHandle != 0;
            
        }

        public void WeaponFire(EventWeaponFire weaponFire)
        {
            if (weaponFire.Owner != _entityView.EntityRef) return;
                
            if (weaponFire.Type == EWeaponType.MainWeapon)
            {
                if (MainWeapon != null && !_weaponDelayDic[MainWeapon])
                {
                    StartCoroutine(ProcessAttack(MainWeapon));
                }
            }
            else if (weaponFire.Type == EWeaponType.SubWeapon)
            {
                if (SubWeapon != null && !_weaponDelayDic[SubWeapon])
                {
                    StartCoroutine(ProcessAttack(SubWeapon));
                }
            }
  
        }
        
        IEnumerator ProcessAttack(Weapon weapon)
        {
            if (_weaponDelayDic[weapon]) yield break;
            _weaponDelayDic[weapon] = true;

            yield return new WaitForSeconds(weapon.AttackInitDelay);

            weapon.OnAttack();

            yield return new WaitForSeconds(weapon.AttackCooltime);

            weapon.OnReadyAttack(true);

            _weaponDelayDic[weapon] = false;
        }
        

    }

}
