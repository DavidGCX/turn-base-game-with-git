using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    protected Unit unit;
    protected bool IsActive;
    [SerializeField] protected Animator animator; 

    protected Action OnActionComplete;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
}
