using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechAttack : MonoBehaviour
{
    public Animator Aniamtor;
    public Button AttackButton;
    public Weapon Weapon;

    int _targetHandle;
    public int TargetHandle => _targetHandle;

    static int playerHandle = 0;
    public int Handle { get; private set; }

    private bool _isAttack;
    
    public bool IsAttack => _isAttack;

    private void OnEnable()
    {
        playerHandle++;
        Handle = playerHandle;
    }

    public void Initialized(Mech mech)
    {
        if (AttackButton != null)
        {
            AttackButton.onClick.AddListener(() =>
            {
                if (Weapon != null && !_isAttack)
                {
                    StartCoroutine(ProcessAttack(mech));
                }
            });
        }
    }

    public void OnUpdate()
    {
        Aniamtor.SetBool("IsAttack", _isAttack);

        if (AttackButton != null)
        {
            AttackButton.interactable = _targetHandle != 0;
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

        // Ÿ���� ������
        if (_targetHandle != enemyHandle)
        {
            // ���� Ÿ���� �ƿ������� ����
            var prevTarget = mech.Map.GetEnemy(_targetHandle);
            if (prevTarget != null)
            {
                var outline = prevTarget.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = false;
                }
            }
            // ���ο� Ÿ���� �ƿ������� ǥ��
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

    IEnumerator ProcessAttack(Mech mech)
    {
        if (_isAttack) yield break;
        _isAttack = true;

        yield return new WaitForSeconds(mech.AttackInitDelay);

        Weapon.OnAttack();

        yield return new WaitForSeconds(mech.AttackSpeed);

        _isAttack = false;
    }
}
