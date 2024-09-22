using Quantum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTowerView : QuantumEntityViewComponent
{
    public Transform Model;
    public Animator Animator;
    public Animator BeaconAnimator;
    public Transform TowerModel;

    public AudioClip fireClip;
    public AudioClip activeClip;
    public AudioClip deactiveClip;

    private Dictionary<string, Transform> _nexusModelDic = new();

    private Frame f;

    private bool _isFire;
    private bool _isAcitve;

    private void Awake()
    {
        QuantumEvent.Subscribe<EventTowerActivate>(this, TowerActivate);
        QuantumEvent.Subscribe<EventTowerAttack>(this, TowerAttack);

        f = QuantumRunner.DefaultGame.Frames.Predicted;
    }

    private void TowerAttack(EventTowerAttack e)
    {
        var team = e.Team;

        BeaconAnimator.SetTrigger($"{team}");

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

        StartCoroutine(RotateTowardsAndFire(nexusModel, e.FirstDelayTime.AsFloat, () =>
        {
            Animator.SetTrigger("Fire");
            AudioManager.Instance.PlaySfx(fireClip);
        }));
    }

    private void TowerActivate(EventTowerActivate e)
    {
        _isAcitve = e.isActive;
        Animator.SetBool("Activate", _isAcitve);
        //BeaconAnimator.SetTrigger($"{e.team}");
        
        if (_isAcitve)
        {
            AudioManager.Instance.PlaySfx(activeClip);
        }
        else
        {
            AudioManager.Instance.PlaySfx(deactiveClip);
        }
    }

    private IEnumerator RotateTowardsAndFire(Transform nexusModel, float rotationDuration, Action callback = null)
    {
        if (!_isAcitve) yield break;
        if (_isFire) yield break;
        _isFire = true;

        Vector3 direction = (nexusModel.position - TowerModel.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float elapsedTime = 0f;

        Quaternion initialRotation = TowerModel.rotation;

        while (elapsedTime < rotationDuration)
        {
            TowerModel.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        TowerModel.rotation = targetRotation; // ¸¶Áö¸·¿¡ Á¤È®ÇÑ Å¸°Ù ¹æÇâÀ¸·Î ¸ÂÃçÁÜ

        callback?.Invoke();

        _isFire = false;
    }

    private void OnDestroy()
    {
        QuantumEvent.UnsubscribeListener(this);
    }
}
