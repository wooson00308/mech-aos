using Quantum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTowerView : QuantumEntityViewComponent
{
    public Transform Model;
    public Animator Animator;
    public Transform TowerModel;

    private Dictionary<string, Transform> _nexusModelDic = new();

    private Frame f;

    public override void OnActivate(Frame frame)
    {
        QuantumEvent.Subscribe<EventTowerActivate>(this, TowerActivate);
        QuantumEvent.Subscribe<EventTowerAttack>(this, TowerAttack);

        f = frame;
    }

    private void TowerAttack(EventTowerAttack e)
    {
        var nexus = e.nexus;
        Transform nexusModel;
        string entityId = nexus.ToString();
        if (_nexusModelDic.TryGetValue(entityId, out Transform getModel))
        {
            nexusModel = getModel;
        }
        else
        {
            var obj = GameObject.Find(entityId);
            if (obj == null) return;
            nexusModel = obj.transform;
            _nexusModelDic.Add(entityId, nexusModel);
        }

        StartCoroutine(RotateTowardsAndFire(nexusModel, () =>
        {
            Animator.SetTrigger("Fire");
            f.Signals.OnNexusHit(e.bullet, e.nexus, e.damage);
        }));
    }

    private void TowerActivate(EventTowerActivate e)
    {
        Animator.SetBool("Activate", e.isActive);
    }

    private IEnumerator RotateTowardsAndFire(Transform nexusModel, Action callback = null)
    {
        Vector3 direction = (nexusModel.position - TowerModel.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float rotationSpeed = 10f;
        while (Quaternion.Angle(TowerModel.rotation, targetRotation) > 0.1f)
        {
            TowerModel.rotation = Quaternion.Lerp(TowerModel.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null; 
        }
        TowerModel.rotation = targetRotation;

        callback?.Invoke();
    }

    private void OnDestroy()
    {
        QuantumEvent.UnsubscribeListener(this);
    }
}
