using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MechMovement : MonoBehaviour
{
    public Animator Animator;
    public NavMeshAgent Agent;
    public Joystick Joystick;
    public ParticleSystem MoveParticle; // ��ƼŬ �ý��� �ʵ� �߰�

    private Vector3 _currentMoveDir;
    public Vector3 CurrentMoveDir => _currentMoveDir;

    public AudioClip moveClip;
    private bool isMoving = false;

    private float sfxPlayIntervalBase = 0.55f; // SFX �⺻ ��� ����
    private float moveAnimationMultiplierBase = 0.7f; // �⺻ �ִϸ��̼� �ӵ�
    private float moveSpeedMultiplierBase = 10f; // �⺻ �̵� �ӵ�

    private float sfxPlayInterval;
    private float moveAnimationMultiplier;
    private float moveSpeedMultiplier;

    public void OnUpdate(Mech mech)
    {
        UpdateMultipliers(mech.MoveSpeed);
        Animator.SetFloat("MoveSpeed", mech.MoveSpeed * moveAnimationMultiplier);
        Move(mech);

        if (!isMoving && _currentMoveDir != Vector3.zero)
        {
            isMoving = true;
            StartCoroutine(PlayMoveSfx(mech));
        }

        if (isMoving && _currentMoveDir == Vector3.zero)
        {
            AudioManager.Instance.StopSfx(moveClip);
            isMoving = false;
        }

        if (mech.Attack.TargetHandle != 0)
        {
            var target = mech.Map.GetEnemy(mech.Attack.TargetHandle);
            var targetDir = target.transform.position - this.transform.position;
            RotateToTargetDir(mech, targetDir);

            // �̵� ���⿡ �´� �ִϸ��̼� ����
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
                // ������ �޸��� �ִϸ��̼�
                Animator.SetFloat("X", 0);
                Animator.SetFloat("Y", 1);

                RotateToDir(mech, _currentMoveDir);
            }
            else
            {
                // ���ִ� �ִϸ��̼�
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

        var offset = _currentMoveDir * (mech.MoveSpeed * moveSpeedMultiplier) * Time.fixedDeltaTime;

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

    private IEnumerator PlayMoveSfx(Mech mech)
    {
        while (isMoving)
        {
            if (MoveParticle != null) MoveParticle.Play(); // ��ƼŬ ���
            AudioManager.Instance.PlaySfx(moveClip, false, mech.MoveSpeed, .3f);
            yield return new WaitForSeconds(sfxPlayInterval);
        }
    }

    private void UpdateMultipliers(float moveSpeed)
    {
        sfxPlayInterval = sfxPlayIntervalBase / moveSpeed;
        moveAnimationMultiplier = moveAnimationMultiplierBase * moveSpeed;
        moveSpeedMultiplier = moveSpeedMultiplierBase * moveSpeed;
    }
}
