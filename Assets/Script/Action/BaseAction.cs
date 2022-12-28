using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    [SerializeField] protected int actionPointRequirement = 1;

    [SerializeField] protected int effectiveDistance = 4;
    protected Unit unit;
    protected bool IsActive;
    private float CIRCLE_ADJUST_FACTOR = 0.3f;
    protected string nameOfAction;
    [SerializeField] protected Animator animator; 

    protected Action OnActionComplete;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        IsActive = false;
    }

    public string GetActionName() => nameOfAction;

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual void StartAction(Action onActionComplete) {
        this.OnActionComplete = onActionComplete;
        IsActive = true;
    }

    public virtual void EndAction() {
        OnActionComplete();
        IsActive = false;
    }
    public abstract List<GridPosition> GetValidGridPositionList();

    public virtual bool IsValidMoveGridPosition(GridPosition gridPosition) => GetValidGridPositionList().Contains(gridPosition);

    public int GetActionSpent() => actionPointRequirement;

    public virtual bool InvalidDistance(GridPosition resultGridPos) {
        Vector3 testPoint = LevelGrid.instance.GetWorldPosition(resultGridPos);
        float testDistance = Vector3.Distance(testPoint, unit.GetWorldPosition());
        return Mathf.RoundToInt(testDistance-CIRCLE_ADJUST_FACTOR) > effectiveDistance * LevelGrid.instance.GetCellSize();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0.1f, 0),effectiveDistance * LevelGrid.instance.GetCellSize());
    }
    
}
