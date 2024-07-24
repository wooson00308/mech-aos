using System.Collections.Generic;
using System.Linq;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum.Mech
{
    public class Weapon : QuantumEntityViewComponent<CustomViewContext>
    {
        private List<ParticleSystem> _weaponFxs;
        public SkinnedMeshRenderer Model;
        public SkinnedMeshRenderer EmptyModel;

        public float AttackCooltime;
        public float AttackInitDelay;

        public AudioClip attackSound;
        public AudioClip readySound;
        
        

        public override void OnActivate(Frame frame)
        {
            _weaponFxs = transform.GetComponentsInChildren<ParticleSystem>().ToList();
        }

        // public override void OnUpdateView()
        // {
        //     base.OnUpdateView();
        //     
        //     foreach (var system in _weaponFxs)
        //     {
        //         var particles = new ParticleSystem.Particle[system.particleCount];
        //         system.GetParticles(particles);
        //         
        //         foreach (var particle in particles)
        //         {
        //             var particlePosition = FPMathUtils.ToFPVector3(particle.position);
        //             
        //             var collisionBox = new FPBounds3(particlePosition, new FPVector3(FP._0_10, FP._0_10, FP._0_10));
        //             return f.Physics.OverlapBox(collisionBox).Length > 0;
        //             
        //             
        //             // 충돌 감지
        //             var collisionDetected = QuantumCollisionDetection(f, particlePosition);
        //         
        //             if (collisionDetected)
        //             {
        //                 f.Events->Add(new ParticleCollisionEvent { CollisionPosition = particlePosition });
        //             }
        //         }
        //         
        //     }
        //     
        // }
        // private bool QuantumCollisionDetection(Frame f, FPVector3 particlePosition)
        // {
        //     var collisionBox = new FPBounds3(particlePosition, new FPVector3(FP._0_10, FP._0_10, FP._0_10));
        //     return f.Physics3D.OverlapShape(particlePosition).Length > 0;
        // }
        public void SetIgnoreCollision(LayerMask mask)
        {
            foreach (var fx in _weaponFxs)
            {
                var collision = fx.collision;
                collision.collidesWith = ~mask;
            }
            
            
            
        }
        public void OnAttack()
        {
            // if (WeaponFx.isPlaying) return;
            foreach (var fx in _weaponFxs)
            {
                fx.Play();
            }

            AudioManager.Instance.PlaySfx(attackSound);
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

