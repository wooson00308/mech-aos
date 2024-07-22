using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechAttack : MonoBehaviour
{
    public Animator Aniamtor;

    public Button MainAttackButton;
    public Weapon MainWeapon;

    public Button SubAttackButton;
    public Weapon SubWeapon;

    int _targetHandle;
    public int TargetHandle => _targetHandle;

    static int playerHandle = 0;
    public int Handle { get; private set; }

    private Dictionary<Weapon, bool> _weaponDelayDic = new();

    private void OnEnable()
    {
        playerHandle++;
        Handle = playerHandle;
    }

    public void Initialized(Mech mech)
    {
        if (MainAttackButton != null)
        {
            _weaponDelayDic.Add(MainWeapon, false);
            MainAttackButton.onClick.AddListener(() =>
            {
                if (MainWeapon != null && !_weaponDelayDic[MainWeapon])
                {
                    StartCoroutine(ProcessAttack(mech, MainWeapon));
                }
            });
        }
        if (SubAttackButton != null)
        {
            _weaponDelayDic.Add(SubWeapon, false);
            SubAttackButton.onClick.AddListener(() =>
            {
                if (SubWeapon != null && !_weaponDelayDic[SubWeapon])
                {
                    StartCoroutine(ProcessAttack(mech, SubWeapon));
                }
            });
        }
    }

    public void OnUpdate()
    {
        Aniamtor.SetBool("IsAttack", _weaponDelayDic[MainWeapon]);

        if (MainAttackButton != null)
        {
            MainAttackButton.interactable = _targetHandle != 0;
            SubAttackButton.interactable = _targetHandle != 0;
        }
    }

    public void OnUpdateTarget(Mech mech)
    {
        var enemy = mech.Map.GetNearestEnemy(Handle, transform.position, mech.AttackRange);

        int enemyHandle;
        if (enemy == null)
        {
            enemyHandle = 0;
        }
        else
        {
            enemyHandle = enemy.Attack.Handle;
        }

        // 타겟이 변했음
        if (_targetHandle != enemyHandle)
        {
            // 이전 타겟의 아웃라인을 제거
            var prevTarget = mech.Map.GetEnemy(_targetHandle);
            if (prevTarget != null)
            {
                var outline = prevTarget.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = false;
                }
            }
            // 새로운 타겟의 아웃라인을 표시
            if (enemy != null)
            {
                var outline = enemy.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                }
            }

            _targetHandle = enemyHandle;
        }
    }

    IEnumerator ProcessAttack(Mech mech, Weapon weapon)
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
