using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Mech : MonoBehaviour
{
	[SerializeField] float _moveSpeed;
	[SerializeField] float _rotationSpeed;
    [SerializeField] float _attackInitDelay;
    [SerializeField] float _attackRange;
    [SerializeField] float _attackDir;
    [SerializeField] float _attackSpeed;

    public float MoveSpeed => _moveSpeed;
    public float RotationSpeed => _rotationSpeed;
    public float AttackInitDelay => _attackInitDelay;
    public float AttackRange => _attackRange;
    public float AttackDir => _attackDir;
    public float AttackSpeed => _attackSpeed;

    [Header("Services")]
    public MechMovement Move;
    public MechAttack Attack;

    Map _map;
    public Map Map => _map;

    private void Awake()
    {
        _map = Map.GetMap();

        Attack.Initialized(this);
    }

    private void FixedUpdate()
    {
        Attack.OnUpdate();
        Move.OnUpdate(this);
    }


    private void Update()
    {
        Attack.OnUpdateTarget(this);
    }
}
