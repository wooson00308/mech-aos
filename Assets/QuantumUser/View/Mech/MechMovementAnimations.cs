using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum.Mech
{
    public class MechMovementAnimations : QuantumCallbacks
    {
        private QuantumEntityView _entityView;
        private Animator _animator;
        public ParticleSystem MoveParticle; // 파티클 시스템 필드 추가

        
        private float sfxPlayIntervalBase = 0.55f; // SFX 기본 재생 간격
        private float moveAnimationMultiplierBase = 0.7f; // 기본 애니메이션 속도
        private float moveSpeedMultiplierBase = 10f; // 기본 이동 속도
        
        private float sfxPlayInterval;
        private float moveAnimationMultiplier;
        private float moveSpeedMultiplier;

        public AudioClip moveClip;
        private bool isMoving = false;
        // Start is called before the first frame update
        void Awake()
        {
            _entityView = GetComponent<QuantumEntityView>();
            _animator = GetComponentInChildren<Animator>();

            // TODO 파괴시 삭제해야하나?
            // QuantumEvent.Subscribe<EventInput>(this, OnInput);

        }

        public override void OnUpdateView(QuantumGame game) {
            
            var frame = game.Frames.Predicted;
            // TODO 캐싱하는게 좋지 않을까?
            var config = frame.FindAsset(frame.RuntimeConfig.MechGameConfig);
            var body = frame.Get<PhysicsBody3D>(_entityView.EntityRef);

            // TODO 무브 스피드 이상함 쏜과 대화 해봐야함.
            var moveSpeed = config.MechMovementSpeed.AsFloat * 0.001f;
            UpdateMultipliers(moveSpeed);
            _animator.SetFloat("MoveSpeed", moveSpeed * moveAnimationMultiplier);
            
            var normalized = body.Velocity.Normalized;

            if (!isMoving && normalized != FPVector3.Zero)
            {
                isMoving = true;
                StartCoroutine(PlayMoveSfx(moveSpeed));
            }
            
            if (isMoving && normalized != FPVector3.Zero)
            {
                AudioManager.Instance.StopSfx(moveClip);
                isMoving = false;
            }
            
            
            if (normalized.XZ.SqrMagnitude == 0)
            {

                _animator.SetFloat("X", 0); 
                _animator.SetFloat("Y", 0);
            }
            else
            {
                _animator.SetFloat("X", 0); 
                _animator.SetFloat("Y", 1);
                RotateToDir(normalized.ToUnityVector3());
                
            }

        }

        private void RotateToDir(Vector3 moveDir)
        {
            var lookDir = Quaternion.LookRotation(moveDir);
            var t = Mathf.Clamp01(11f * Time.fixedDeltaTime * 1.2f);
            _animator.transform.rotation = Quaternion.Lerp(_animator.transform.rotation, lookDir, t);
        }
        private void UpdateMultipliers(float moveSpeed)
        {
            sfxPlayInterval = sfxPlayIntervalBase / moveSpeed;
            moveAnimationMultiplier = moveAnimationMultiplierBase * moveSpeed;
            moveSpeedMultiplier = moveSpeedMultiplierBase * moveSpeed;
        }
        
        private IEnumerator PlayMoveSfx(float speed)
        {
            while (isMoving)
            {
                if (MoveParticle != null) MoveParticle.Play(); // 파티클 재생
                AudioManager.Instance.PlaySfx(moveClip, false, speed, .3f);
                yield return new WaitForSeconds(sfxPlayInterval);
            }
        }

    }
}

