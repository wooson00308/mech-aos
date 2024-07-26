using Photon.Deterministic;
using UnityEngine;

namespace Quantum.Mech
{
    public static unsafe class RespawnHelper
    {
        public static void RespawnMechanic(Frame frame, EntityRef mechanic)
        {
            FPVector3 position = FPVector3.One * 4;
            PlayableMechanic* playableMechanic = frame.Unsafe.GetPointer<PlayableMechanic>(mechanic);
            
            int spawnCount = frame.ComponentCount<SpawnIdentifier>();
            
            if (spawnCount != 0)
            {
                
                foreach (var (spawn, spawnIdentifier) in frame.Unsafe.GetComponentBlockIterator<SpawnIdentifier>())
                {
                    
                    if (spawnIdentifier->Team == playableMechanic->Team)
                    {
                        Transform3D spawnTransform = frame.Get<Transform3D>(spawn);
                        position = spawnTransform.Position;
                        break;
                    }
                }
            }

            Transform3D* transform = frame.Unsafe.GetPointer<Transform3D>(mechanic);
            PhysicsCollider3D* collider = frame.Unsafe.GetPointer<PhysicsCollider3D>(mechanic);

            transform->Position = position;
            transform->Teleport(frame,position);
            collider->IsTrigger = false;

            frame.Signals.OnMechanicRespawn(mechanic);
            frame.Events.OnMechanicRespawn(mechanic);
        }
    }
}