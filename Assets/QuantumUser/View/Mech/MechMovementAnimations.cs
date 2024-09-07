using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum.Mech
{
    public class MechMovementAnimations : QuantumEntityViewComponent<CustomViewContext>
    {
        // private QuantumEntityView _entityView;
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

        private MechGameConfig _config;
        public override void OnActivate(Frame frame)
        {
            // _entityView = GetComponent<QuantumEntityView>();
            _animator = GetComponentInChildren<Animator>();
            _config = frame.FindAsset(frame.RuntimeConfig.MechGameConfig);
        }

        public override void OnUpdateView() {
            
     
            var body = VerifiedFrame.Get<CharacterController3D>(EntityView.EntityRef);
            // TODO 무브 스피드 이상함 쏜과 대화 해봐야함.
            var moveSpeed = _config.MechMovementSpeed.AsFloat * 0.001f;
            UpdateMultipliers(moveSpeed);
            _animator.SetFloat("MoveSpeed", moveSpeed * moveAnimationMultiplier);
            
            var normalized = body.Velocity.Normalized;
            // Debug.Log(normalized);
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
            }

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

