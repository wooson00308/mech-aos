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
        private List<EntityRef> _nexusEntityRefs = new(); // Nexus EntityRef 리스트
        private int _currentNexusIndex = 0;               // 현재 타워의 인덱스
        private bool _isOccupy;
        private bool _isUpdatedOccupy;
        private FP _latency = 5;
        private FP _latencyElapsedTime = 0;
        private FP _runningElapsedTime = 0;

        public override void OnInit(Frame f)
        {
            base.OnInit(f);
            _nexusComponents = f.Unsafe.GetComponentBlockIterator<Nexus>();

            // Nexus EntityRef 리스트 초기화
            foreach (var entityComponentPointerPair in _nexusComponents)
            {
                if (!entityComponentPointerPair.Component->IsDestroy)
                {
                    _nexusEntityRefs.Add(entityComponentPointerPair.Entity); // EntityRef만 저장
                }
            }
        }

        public override void Update(Frame f, ref Filter filter)
        {
            if (_isOccupy)
            {
                if (!_isUpdatedOccupy)
                {
                    _isUpdatedOccupy = true;
                    f.Events.TowerActivate(true);
                }

                if (_latencyElapsedTime < _latency)
                {
                    _latencyElapsedTime += f.DeltaTime;
                    Debug.Log($"CenterTower Process Charge {_latencyElapsedTime} / {_latency}");
                }

                if (_latencyElapsedTime >= _latency)
                {
                    _runningElapsedTime += f.DeltaTime;
                    Debug.Log($"CenterTower Attack Charge {_runningElapsedTime} / {filter.CenterTowerFields->Time}");
                }

                if (_runningElapsedTime >= filter.CenterTowerFields->Time)
                {
                    if (enterEntityRefs.Count <= 0 || _nexusEntityRefs.Count <= 0) return;

                    // 현재 타워 인덱스를 기준으로 공격
                    var currentNexusEntity = _nexusEntityRefs[_currentNexusIndex];
                    var currentNexus = f.Unsafe.GetPointer<Nexus>(currentNexusEntity); // 포인터로 변환

                    var playableMechanic = f.Unsafe.GetPointer<PlayableMechanic>(enterEntityRefs[0]);
                    if (currentNexus->Team == playableMechanic->Team)
                    {
                        _currentNexusIndex = (_currentNexusIndex + 1) % _nexusEntityRefs.Count;
                        return;
                    }

                    _runningElapsedTime -= filter.CenterTowerFields->Time;

                    Debug.Log($"CenterTower Attack Complete: {currentNexus->Team} -> {filter.CenterTowerFields->Damage}");
                    f.Events.TowerAttack(filter.Entity, currentNexusEntity, filter.CenterTowerFields->Damage);

                    // 인덱스를 순환시키기 위한 로직
                    _currentNexusIndex = (_currentNexusIndex + 1) % _nexusEntityRefs.Count;
                }
            }
            else
            {
                if (_isUpdatedOccupy)
                {
                    _isUpdatedOccupy = false;
                    f.Events.TowerActivate(false);
                }
            }
        }

        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info)
        {
            if (f.Has<FootboardIdentifier>(info.Entity) && f.Has<PlayableMechanic>(info.Other))
            {
                if (enterEntityRefs.Contains(info.Other)) return;
                enterEntityRefs.Add(info.Other);
                _isOccupy = true;
                Debug.Log("CenterTower Start!");
            }
            else if (f.Has<FootboardIdentifier>(info.Other) && f.Has<PlayableMechanic>(info.Entity))
            {
                if (enterEntityRefs.Contains(info.Entity)) return;
                enterEntityRefs.Add(info.Entity);
                _isOccupy = true;
                Debug.Log("CenterTower Start!");
            }
        }

        public void OnTriggerExit3D(Frame f, ExitInfo3D info)
        {
            if (f.Has<FootboardIdentifier>(info.Entity) && f.Has<PlayableMechanic>(info.Other))
            {
                if (!enterEntityRefs.Contains(info.Other)) return;
                if (enterEntityRefs[0] == info.Other)
                {
                    _latencyElapsedTime = 0;
                    _runningElapsedTime = 0;
                    _currentNexusIndex = 0; // 타워 공격 순서를 초기화
                    Debug.Log("CenterTower Changed!");
                }
                enterEntityRefs.Remove(info.Other);
                if (enterEntityRefs.Count <= 0)
                {
                    _isOccupy = false;
                    Debug.Log("CenterTower End!");
                }
            }
            else if (f.Has<FootboardIdentifier>(info.Other) && f.Has<PlayableMechanic>(info.Entity))
            {
                if (!enterEntityRefs.Contains(info.Entity)) return;
                if (enterEntityRefs[0] == info.Entity)
                {
                    _latencyElapsedTime = 0;
                    _runningElapsedTime = 0;
                    _currentNexusIndex = 0; // 타워 공격 순서를 초기화
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
