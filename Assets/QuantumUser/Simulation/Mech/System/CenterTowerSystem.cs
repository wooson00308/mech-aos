using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Core;
using Quantum.Physics3D;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class CenterTowerSystem : SystemMainThreadFilter<CenterTowerSystem.Filter>,
        ISignalOnTriggerEnter3D, ISignalOnTriggerExit3D
    {
        public struct Filter
        {
            public EntityRef Entity;
            public CenterTowerFields* CenterTowerFields;
        }

        private ComponentBlockIterator<Nexus> _nexusComponents;
        private List<EntityRef> enterEntityRefs = new();

        private bool _isOccupy;
        private bool _isUpdatedOccupy;
        private FP _latency = 5;
        private FP _latencyElapsedTime = 0;
        private FP _runningElapsedTime = 0;
        private FP _attackElapsedTime = 0;

        private Nexus* _attackNexus;
        public override void OnInit(Frame f)
        {
            base.OnInit(f);
            _nexusComponents = f.Unsafe.GetComponentBlockIterator<Nexus>();
        }

        public override void Update(Frame f, ref Filter filter)
        {
            if (_isOccupy)
            {
                PlayableMechanic* playableMechanic;
                if (!_isUpdatedOccupy)
                {
                    _isUpdatedOccupy = true;
            
                    playableMechanic = f.Unsafe.GetPointer<PlayableMechanic>(enterEntityRefs[0]);
                    f.Events.TowerActivate(playableMechanic->Team, true);
                }
                if (_latencyElapsedTime < _latency)
                {
                    _latencyElapsedTime += f.DeltaTime;
                    Debug.Log($"CenterTower Process Charge {_latencyElapsedTime} / {_latency}");
                }
                if (_latencyElapsedTime >= _latency)
                {
                    _runningElapsedTime += f.DeltaTime;
                    _attackElapsedTime += f.DeltaTime;
                    Debug.Log($"CenterTower Attack Charge {_runningElapsedTime} / {filter.CenterTowerFields->Time}");

                }
                if (_runningElapsedTime / filter.CenterTowerFields->Time >= 1)
                {
                    _runningElapsedTime -= filter.CenterTowerFields->Time;

                    if (enterEntityRefs.Count <= 0) return;
                    foreach (var entityComponentPointerPair in _nexusComponents)
                    {
                        if (entityComponentPointerPair.Component->IsDestroy) continue;
                        playableMechanic = f.Unsafe.GetPointer<PlayableMechanic>(enterEntityRefs[0]);
                        if (entityComponentPointerPair.Component->Team == playableMechanic->Team) continue;
                        if (_attackNexus == entityComponentPointerPair.Component) continue;
                        Debug.Log($"CenterTower Attack Complete: {entityComponentPointerPair.Component->Team} -> {f.Unsafe.GetPointer<Nexus>(entityComponentPointerPair.Entity)->CurrentHealth - filter.CenterTowerFields->Damage}");
                        _attackNexus = entityComponentPointerPair.Component;
                        f.Events.TowerAttack(filter.Entity, entityComponentPointerPair.Entity, filter.CenterTowerFields->FirstDelayTime, filter.CenterTowerFields->Damage);
                        f.Signals.OnNexusHit(filter.Entity, entityComponentPointerPair.Entity, filter.CenterTowerFields->Damage);

                        //if (_attackElapsedTime / filter.CenterTowerFields->Time >= filter.CenterTowerFields->FirstDelayTime)
                        //{
                        //    _attackElapsedTime -= filter.CenterTowerFields->Time;
                        //}
                        break;
                    }
                }
            }
            else
            {
                if (_isUpdatedOccupy)
                {
                    _isUpdatedOccupy = false;
                    f.Events.TowerActivate(Team.None, false);
                }
            }


        }

        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info)
        {


            if (f.Has<FootboardIdentifier>(info.Entity) &&
                f.Has<PlayableMechanic>(info.Other))
            {
                if (enterEntityRefs.Contains(info.Other)) return;
                enterEntityRefs.Add(info.Other);
                _isOccupy = true;
                Debug.Log("CenterTower Start!");
            }
            else if (f.Has<FootboardIdentifier>(info.Other) &&
                     f.Has<PlayableMechanic>(info.Entity))
            {
                if (enterEntityRefs.Contains(info.Entity)) return;
                enterEntityRefs.Add(info.Entity);
                _isOccupy = true;
                Debug.Log("CenterTower Start!");

            }
        }

        public void OnTriggerExit3D(Frame f, ExitInfo3D info)
        {

            if (f.Has<FootboardIdentifier>(info.Entity) &&
                f.Has<PlayableMechanic>(info.Other))
            {
                if (!enterEntityRefs.Contains(info.Other)) return;
                if (enterEntityRefs[0] == info.Other)
                {
                    _latencyElapsedTime = 0;
                    _runningElapsedTime = 0;
                    _attackNexus = null;
                    Debug.Log("CenterTower Changed!");

                }
                enterEntityRefs.Remove(info.Other);
                if (enterEntityRefs.Count <= 0)
                {
                    _isOccupy = false;
                    Debug.Log("CenterTower End!");

                }

            }
            else if (f.Has<FootboardIdentifier>(info.Other) &&
                     f.Has<PlayableMechanic>(info.Entity))
            {
                if (!enterEntityRefs.Contains(info.Entity)) return;
                if (enterEntityRefs[0] == info.Entity)
                {
                    _latencyElapsedTime = 0;
                    _runningElapsedTime = 0;
                    _attackNexus = null;
                    Debug.Log("CenterTower Changed!");

                }
                enterEntityRefs.Remove(info.Entity);
                if (enterEntityRefs.Count <= 0)
                {
                    _isOccupy = false;
                    Debug.Log("CenterTower End!");

                }
            }
        }
    }
}