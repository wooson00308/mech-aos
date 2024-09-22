using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Collections;
using Quantum.Core;
using Quantum.Physics3D;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class CenterTowerSystem : SystemMainThreadFilter<CenterTowerSystem.Filter>, 
        ISignalOnTriggerEnter3D, ISignalOnTriggerExit3D, ISignalOnGameEnded
    {
        public struct Filter
        {
            public EntityRef Entity;
            public CenterTowerFields* CenterTowerFields;
        }

        public override void OnInit(Frame f)
        {
            base.OnInit(f);
            f.Global->CenterTowerEnterEntityRefs = f.AllocateList<EntityRef>();
        }

        public override void Update(Frame f, ref Filter filter)
        {
            
            if (f.Global->CenterTowerIsOccupy)
            {
                var config = f.FindAsset(f.RuntimeConfig.MechGameConfig);
               
                PlayableMechanic* playableMechanic;
                if (!f.Global->CenterTowerIsUpdatedOccupy)
                {
                    f.Global->CenterTowerIsUpdatedOccupy = true;
            
                    playableMechanic = GetPlayableMechanic(f);
                    if (playableMechanic == null) return;
                    
                    f.Events.TowerActivate(playableMechanic->Team, true);
                }
                if (f.Global->CenterTowerLatencyElapsedTime < config.centerTowerLatency)
                {
                    f.Global->CenterTowerLatencyElapsedTime += f.DeltaTime;
                    // Debug.Log($"CenterTower Process Charge {f.Global->CenterTowerLatencyElapsedTime} / {config.centerTowerLatency}");
                }
                if (f.Global->CenterTowerLatencyElapsedTime >= config.centerTowerLatency)
                {
                    f.Global->CenterTowerRunningElapsedTime += f.DeltaTime;
                    f.Global->CenterTowerAttackElapsedTime += f.DeltaTime;
                    // Debug.Log($"CenterTower Attack Charge {f.Global->CenterTowerRunningElapsedTime} / {filter.CenterTowerFields->Time}");

                }
                if (f.Global->CenterTowerRunningElapsedTime / filter.CenterTowerFields->Time >= 1)
                {
                    f.Global->CenterTowerRunningElapsedTime -= filter.CenterTowerFields->Time;
                    
                    foreach (var entityComponentPointerPair in f.Unsafe.GetComponentBlockIterator<Nexus>())
                    {
                        if (entityComponentPointerPair.Component->IsDestroy) continue;
                        playableMechanic = GetPlayableMechanic(f);
                        if (playableMechanic == null) return;
                        if (entityComponentPointerPair.Component->Team == playableMechanic->Team) continue;
                        if(f.Global->TeamsHitByCentreTower == entityComponentPointerPair.Component->Team) continue;
                        f.Global->TeamsHitByCentreTower = entityComponentPointerPair.Component->Team;
                        // Debug.Log($"CenterTower Attack Complete: {entityComponentPointerPair.Component->Team} -> {f.Unsafe.GetPointer<Nexus>(entityComponentPointerPair.Entity)->CurrentHealth - filter.CenterTowerFields->Damage}");
                        f.Events.TowerAttack(filter.Entity, entityComponentPointerPair.Entity, filter.CenterTowerFields->FirstDelayTime, filter.CenterTowerFields->Damage);
                        f.Signals.OnNexusHit(filter.Entity, entityComponentPointerPair.Entity, filter.CenterTowerFields->Damage);
                        
                        break;
                    }
                }
            }
            else
            {
                if (f.Global->CenterTowerIsUpdatedOccupy)
                {
                    f.Global->CenterTowerIsUpdatedOccupy = false;
                    f.Events.TowerActivate(Team.None, false);
                }
            }
        }

        public PlayableMechanic* GetPlayableMechanic(Frame f)
        {
            var enterEntityRefs = f.ResolveList(f.Global->CenterTowerEnterEntityRefs);
            if (enterEntityRefs.Count <= 0) return null;
            var zero = enterEntityRefs[0].ToString();
            for (int i = enterEntityRefs.Count - 1; i >= 0; i--)
            {
                Debug.Log($"{enterEntityRefs.Count}/ {enterEntityRefs[i]}");
                var status =  f.Unsafe.GetPointer<Status>(enterEntityRefs[i]);
                if(status->IsDead) enterEntityRefs.RemoveAt(i);
            }

            if (enterEntityRefs.Count <= 0)
            {
                f.Global->CenterTowerLatencyElapsedTime = 0;
                f.Global->CenterTowerRunningElapsedTime = 0;
                f.Global->TeamsHitByCentreTower = Team.None;
                f.Global->CenterTowerIsOccupy = false;
                return null;
            }

            if (zero == enterEntityRefs[0].ToString()) return f.Unsafe.GetPointer<PlayableMechanic>(enterEntityRefs[0]);
            
            f.Global->CenterTowerLatencyElapsedTime = 0;
            f.Global->CenterTowerRunningElapsedTime = 0;
            f.Global->TeamsHitByCentreTower = Team.None;
            return f.Unsafe.GetPointer<PlayableMechanic>(enterEntityRefs[0]);
        }
        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info)
        {


            if (f.Has<FootboardIdentifier>(info.Entity) &&
                f.Has<PlayableMechanic>(info.Other))
            {
                var enterEntityRefs = f.ResolveList(f.Global->CenterTowerEnterEntityRefs);
                
                if (enterEntityRefs.Contains(info.Other)) return;
                var status = f.Unsafe.GetPointer<Status>(info.Other);
                if (status->IsDead) return;
                enterEntityRefs.Add(info.Other);
                Debug.Log($"{enterEntityRefs[0]} / {info.Other}");
                f.Global->CenterTowerIsOccupy = true;
                Debug.Log("CenterTower Start!");
            }
            else if (f.Has<FootboardIdentifier>(info.Other) &&
                     f.Has<PlayableMechanic>(info.Entity))
            {
                var enterEntityRefs = f.ResolveList(f.Global->CenterTowerEnterEntityRefs);
                if (enterEntityRefs.Contains(info.Entity)) return;
                var status = f.Unsafe.GetPointer<Status>(info.Entity);
                if (status->IsDead) return;
                enterEntityRefs.Add(info.Entity);
                Debug.Log($"{enterEntityRefs[0]} / {info.Entity}");

                f.Global->CenterTowerIsOccupy = true;
                Debug.Log("CenterTower Start!");

            }
        }

        public void OnTriggerExit3D(Frame f, ExitInfo3D info)
        {

            if (f.Has<FootboardIdentifier>(info.Entity) &&
                f.Has<PlayableMechanic>(info.Other))
            {
                var enterEntityRefs = f.ResolveList(f.Global->CenterTowerEnterEntityRefs);
                if (!enterEntityRefs.Contains(info.Other)) return;
                if (enterEntityRefs[0] == info.Other)
                {
                    f.Global->CenterTowerLatencyElapsedTime = 0;
                    f.Global->CenterTowerRunningElapsedTime = 0;
                    f.Global->TeamsHitByCentreTower = Team.None;
                    Debug.Log("CenterTower Changed!");

                }
                enterEntityRefs.Remove(info.Other);
                if (enterEntityRefs.Count <= 0)
                {
                    f.Global->CenterTowerIsOccupy = false;
                    Debug.Log("CenterTower End!");

                }

            }
            else if (f.Has<FootboardIdentifier>(info.Other) &&
                     f.Has<PlayableMechanic>(info.Entity))
            {
                var enterEntityRefs = f.ResolveList(f.Global->CenterTowerEnterEntityRefs);
                if (!enterEntityRefs.Contains(info.Entity)) return;
                if (enterEntityRefs[0] == info.Entity)
                {
                    f.Global->CenterTowerLatencyElapsedTime = 0;
                    f.Global->CenterTowerRunningElapsedTime = 0;
                    f.Global->TeamsHitByCentreTower = Team.None;
                    Debug.Log("CenterTower Changed!");

                }
                enterEntityRefs.Remove(info.Entity);
                if (enterEntityRefs.Count <= 0)
                {
                    f.Global->CenterTowerIsOccupy = false;
                    Debug.Log("CenterTower End!");

                }
            }
        }

        public void OnGameEnded(Frame f, GameController* gameController)
        {
            f.FreeList(f.Global->CenterTowerEnterEntityRefs);
            f.SystemDisable<WeaponSystem>();
        }
    }
}