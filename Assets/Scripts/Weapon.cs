using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum.Mech
{
    public unsafe class Weapon : QuantumEntityViewComponent<CustomViewContext>
    {
        private List<ParticleSystem> _weaponFxs;
        public List<ParticleSystem> Bullets;
        
        public SkinnedMeshRenderer Model;
        public SkinnedMeshRenderer EmptyModel;

        public float AttackCooltime;
        public float AttackInitDelay;

        public AudioClip attackSound;
        public AudioClip readySound;

        public Shape3DConfig Config;
        public BulletData BulletData;

        public override void OnActivate(Frame frame)
        {
            _weaponFxs = transform.GetComponentsInChildren<ParticleSystem>().ToList();

            foreach (var system in _weaponFxs)
            {
                system.useAutoRandomSeed = false;
                system.randomSeed = UInt32.MinValue;
            }
        }

        public override void OnUpdateView()
        {
            var frame = Game.Frames.Predicted;
            foreach (var system in Bullets)
            {
                var particles = new ParticleSystem.Particle[system.particleCount];
                system.GetParticles(particles);
                foreach (var particle in particles)
                {
                    
                    var particlePosition = FPMathUtils.ToFPVector3(particle.position);
                    var particleRotation3D = particle.rotation3D.ToFPVector3();
                    Physics3D.HitCollection3D hits = frame.Physics3D.OverlapShape(particlePosition, FPQuaternion.Euler(particleRotation3D), Config.CreateShape(frame), frame.Layers.GetLayerMask("Enemy"));
                    
                    
                    for (int i = 0; i < hits.Count; i++)
                    {
                        var entity = hits[i].Entity;
                        Debug.Log($"{entity} / {entity.Index}");
                        
                        if (entity != EntityRef.None)
                        {
                            // frame.Get<EntityRef>(EntityPrototype.);
                            frame.Signals.OnMechanicHit(EntityRef, entity, BulletData.Damage);
                            var isDestroy = frame.Destroy(EntityRef);
                            Debug.Log($"삭제 : {isDestroy}");

                        }

                        // BulletFields fields = frame.Get<BulletFields>(EntityPrototype);
                        // FPVector3 position = frame.Get<Transform3D>(EntityPrototype).Position;
                        // frame.Events.OnBulletDestroyed(EntityPrototype.GetHashCode(), fields.Source, position, fields.Direction, fields.BulletData);
                        // frame.Destroy(EntityPrototype);
                    }
                    
                }
                
            }
            
        }
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

