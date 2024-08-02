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

        int _targetHandle;
        public int TargetHandle => _targetHandle;

        static int playerHandle = 0;
        public int Handle { get; private set; }
        
        private Dictionary<string, Weapon> _weaponDelayDic = new();
        private Dictionary<Weapon, bool> IsCoolDowns = new();
        
        
        private bool MainAttackInteractable;
        private bool SubAttackInteractable;

        private void Awake()
        {
            _entityView = GetComponent<QuantumEntityView>();
            var weapons = GetComponentsInChildren<Weapon>(true);
            foreach (var weapon in weapons)
            {
                _weaponDelayDic.Add(weapon.name, weapon);
                IsCoolDowns.Add(weapon,false);
            }
            
            playerHandle++;
            Handle = playerHandle;

            QuantumEvent.Subscribe<EventWeaponFire>(this, WeaponFire);
        }

        public override void OnUpdateView(QuantumGame game)
        {
            // Aniamtor.SetBool("IsAttack", _weaponDelayDic[MainWeapon]);
            Aniamtor.SetBool("IsAttack", true);
        }

        public void WeaponFire(EventWeaponFire weaponFire)
        {
            
            var weaponData = _entityView.Game.Frames.Predicted.FindAsset<WeaponData>(weaponFire.WeaponData.Id);
            if (!_weaponDelayDic.ContainsKey(weaponData.RootName)) return;
            
            if (weaponFire.Owner != _entityView.EntityRef) return;
            var weapon = _weaponDelayDic[weaponData.RootName];
            
            if (weapon != null && !IsCoolDowns[weapon])
            {
                // StartCoroutine(ProcessAttack(weapon, weaponData));
                weapon.OnAttack();
            }

        }
        
        IEnumerator ProcessAttack(Weapon weapon, WeaponData weaponData)
        {
            if (IsCoolDowns[weapon]) yield break;
            IsCoolDowns[weapon] = true;

            yield return new WaitForSeconds((float)(1 / weaponData.FireRate));

            weapon.OnAttack();
            
            Debug.Log($"TimeToRecharge : {(float)weaponData.TimeToRecharge}");

            yield return new WaitForSeconds((float)weaponData.TimeToRecharge);

            weapon.OnReadyAttack(true);

            IsCoolDowns[weapon] = false;
            Debug.Log("차징!");
        }

        public void OnDestroy()
        {
            QuantumEvent.UnsubscribeListener(this);
        }
    }

}
