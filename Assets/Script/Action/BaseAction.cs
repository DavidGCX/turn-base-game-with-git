using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    protected Unit unit;
    protected bool IsActive;

    protected string name;
    [SerializeField] protected Animator animator; 

    protected Action OnActionComplete;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
    public abstract List<GridPosition> GetValidGridPositionList();

    public virtual bool IsValidMoveGridPosition(GridPosition gridPosition) => GetValidGridPositionList().Contains(gridPosition);

    
}
