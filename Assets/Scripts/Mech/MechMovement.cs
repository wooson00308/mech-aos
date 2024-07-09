using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MechMovement : MonoBehaviour
{
    public Animator Animator;
    public NavMeshAgent Agent;
    public Joystick Joystick;

    private Vector3 _currentMoveDir;
    public Vector3 CurrentMoveDir => _currentMoveDir;

    public void OnUpdate(Mech mech)
    {
        Move(mech);
        
        if (mech.Attack.TargetHandle != 0)
        {
            var target = mech.Map.GetEnemy(mech.Attack.TargetHandle);
            var targetDir = target.transform.position - this.transform.position;
            RotateToTargetDir(mech, targetDir);

            // 이동 방향에 맞는 애니메이션 적용
            var targetDirNorm = targetDir.normalized;
            var angle = Mathf.Acos(targetDirNorm.z);
            if (targetDirNorm.x < 0)
            {
                angle = -1 * angle;
            }

            var cos = Mathf.Cos(angle);
            var sin = Mathf.Sin(angle);

            var y = _currentMoveDir.x * sin + _currentMoveDir.z * cos;
            var x = _currentMoveDir.x * cos - _currentMoveDir.z * sin;

            Animator.SetFloat("Y", y);
            Animator.SetFloat("X", x);
        }
        else
        {
            if (_currentMoveDir != Vector3.zero)
            {
                // 앞으로 달리는 애니메이션
                Animator.SetFloat("X", 0);
                Animator.SetFloat("Y", 1);

                RotateToDir(mech, _currentMoveDir);
            }
            else
            {
                // 서있는 애니메이션
                Animator.SetFloat("X", 0);
                Animator.SetFloat("Y", 0);
            }
        }
    }

    private void Move(Mech mech)
    {
        if (Joystick == null) return;

        Vector3 moveDir = new(Joystick.Horizontal(), 0, Joystick.Vertical());
        _currentMoveDir = moveDir.normalized;

        var offset = _currentMoveDir * mech.MoveSpeed * Time.fixedDeltaTime;

        Agent.Move(offset);
    }

    private void RotateToTargetDir(Mech mech, Vector3 targetDir)
    {
        var lookDir = Quaternion.LookRotation(targetDir);

        var additionalRotation = Quaternion.Euler(0, mech.AttackDir, 0);
        lookDir *= additionalRotation;

        var t = Mathf.Clamp01(mech.RotationSpeed * Time.fixedDeltaTime * mech.MoveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookDir, t);
    }

    private void RotateToDir(Mech mech, Vector3 moveDir)
    {
        var lookDir = Quaternion.LookRotation(moveDir);
        var t = Mathf.Clamp01(mech.RotationSpeed * Time.fixedDeltaTime * mech.MoveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookDir, t);
    }
}
