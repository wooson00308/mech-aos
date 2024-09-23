using System;
using System.Collections;
using System.Collections.Generic;
using Quantum;
using Quantum.Mech;
using UnityEngine;

[Serializable]
public class FX
{
    public string name;
    public ParticleSystem particleSystem;
}
public class FXManager : QuantumViewComponent<CustomViewContext>
{
    public static FXManager Instance { get; private set; }
    public List<FX> particleSystems;
    private Dictionary<string, FX> fxCache = new Dictionary<string, FX>();

    public void Awake()
    {
        QuantumEvent.Subscribe(this, (EventOnTrapDestroyed e) => OnTrapDestroyed(e));
        QuantumEvent.Subscribe(this, (EventOnNexusDestroy e) => OnNexusDestroy(e));
        QuantumEvent.Subscribe(this, (EventOnMechanicDeath e) => OnMechanicDeath(e));

    }

    private void OnMechanicDeath(EventOnMechanicDeath e)
    {
        var transform3D = QuantumRunner.DefaultGame.Frames.Predicted.Get<Transform3D>(e.Mechanic);
        SpawnFX("ExplosionFire", transform3D.Position.ToUnityVector3());
    }

    private void OnNexusDestroy(EventOnNexusDestroy e)
    {
        var transform3D = QuantumRunner.DefaultGame.Frames.Predicted.Get<Transform3D>(e.Nexus);
        SpawnFX("ExplosionFire", transform3D.Position.ToUnityVector3());
    }

    private void OnTrapDestroyed(EventOnTrapDestroyed e)
    {
        Debug.Log(e.TrapPosition);
        SpawnFX("OnTrapDestroyed", e.TrapPosition.ToUnityVector3());
    }
    
    public FX GetFXByName(string fxName)
    {
        // 캐시에 이미 FX가 있는지 확인
        if (fxCache.ContainsKey(fxName))
        {
            return fxCache[fxName];
        }

        // 캐시에 없다면 리스트에서 찾아서 캐싱
        FX foundFX = particleSystems.Find(fx => fx.name == fxName);
        if (foundFX != null)
        {
            fxCache[fxName] = foundFX;
        }
        return foundFX;
    }
    public void SpawnFX(string key, Vector3 position)
    {
        var fx = GetFXByName(key);
        if (fx == null)
        {
            Debug.LogError($"{key} 이름의 FX는 없습니다.");
            return;
        }
        var instantiate = Instantiate(fx.particleSystem, position, Quaternion.identity, transform);
        instantiate.Stop();
        var children = instantiate.GetComponentsInChildren<ParticleSystem>();
        if (children != null && children.Length > 0)
        {
            foreach (var system in children)
            {
                var systemModule = system.main;
                systemModule.loop = false;
                systemModule.stopAction = ParticleSystemStopAction.Destroy;
            }
        }
        var main = instantiate.main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;
        instantiate.Play();
    }
}
