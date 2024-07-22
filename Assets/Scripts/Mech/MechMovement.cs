using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechMovement : MonoBehaviour
{
    public Animator Animator;
    public Joystick Joystick;
    public ParticleSystem MoveParticle; // 파티클 시스템 필드 추가

    public float dashDuration = .5f; // 대시 지속 시간
    public float dashSpeedMultiplier = 20f; // 대시 속도 배수

    private Vector3 _currentMoveDir;
    public Vector3 CurrentMoveDir => _currentMoveDir;

    public AudioClip moveClip;
    private bool isMoving = false;
    private bool isDashing = false;

    private float sfxPlayIntervalBase = 0.55f; // SFX 기본 재생 간격
    private float moveAnimationMultiplierBase = 0.7f; // 기본 애니메이션 속도
    private float moveSpeedMultiplierBase = 10f; // 기본 이동 속도

    private float sfxPlayInterval;
    private float moveAnimationMultiplier;
    private float moveSpeedMultiplier;

    public void OnUpdate(Mech mech)
    {
        if (isDashing) return; // 대시 중에는 일반 이동 비활성화

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

            // 이동 방향에 맞는 애니메이션 설정
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
                // 움직임 애니메이션
                Animator.SetFloat("X", 0);
                Animator.SetFloat("Y", 1);

                RotateToDir(mech, _currentMoveDir);
            }
            else
            {
                // 정지 애니메이션
                Animator.SetFloat("X", 0);
                Animator.SetFloat("Y", 0);
            }
        }
    }

    public void OnDash()
    {
        if (isDashing) return;

        StartCoroutine(DashRoutine(transform.forward)); // 모델이 바라보는 방향으로 대시
    }

    private IEnumerator DashRoutine(Vector3 moveVector)
    {
        isDashing = true;

        Animator.SetBool("IsDash", true); // 대시 애니메이션 재생

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            transform.Translate(dashSpeedMultiplier * moveVector * Time.deltaTime, Space.World);
            yield return null;
        }

        Animator.SetBool("IsDash", false); // 대시 애니메이션 재생
        isDashing = false;
    }

    private void Move(Mech mech)
    {
        if (Joystick == null) return;

        Vector3 moveDir = new(Joystick.Horizontal(), 0, Joystick.Vertical());
        _currentMoveDir = moveDir.normalized;

        var offset = _currentMoveDir * (mech.MoveSpeed * moveSpeedMultiplier) * Time.deltaTime;
        transform.Translate(offset, Space.World);
    }

    private void RotateToTargetDir(Mech mech, Vector3 targetDir)
    {
        var lookDir = Quaternion.LookRotation(targetDir);

        var additionalRotation = Quaternion.Euler(0, mech.AttackDir, 0);
        lookDir *= additionalRotation;

        var t = Mathf.Clamp01(mech.RotationSpeed * Time.deltaTime * mech.MoveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookDir, t);
    }

    private void RotateToDir(Mech mech, Vector3 moveDir)
    {
        var lookDir = Quaternion.LookRotation(moveDir);
        var t = Mathf.Clamp01(mech.RotationSpeed * Time.deltaTime * mech.MoveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookDir, t);
    }

    private IEnumerator PlayMoveSfx(Mech mech)
    {
        while (isMoving)
        {
            if (MoveParticle != null) MoveParticle.Play(); // 파티클 재생
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
