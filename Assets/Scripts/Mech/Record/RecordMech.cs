using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordMech : MonoBehaviour
{
    public bool isFire;
    // Start is called before the first frame update
    void Start()
    {
        var animator = GetComponentInChildren<Animator>();
        animator.SetBool("Fire", isFire);
    }
}
