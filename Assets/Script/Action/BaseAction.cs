using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    [Header("action花费的点数")]
    [SerializeField] protected int actionPointRequirement = 1;
    
    [Header("action的最大距离")]

    [Tooltip("默认为近似圆形")]
    [SerializeField] protected int effectiveDistance = 4;
    [Header("action的名字")]
    [SerializeField] protected string nameOfAction;

    [Header("action对应的格子颜色")]
    [Tooltip("在GridVisualController的列表中添加更多选项")]
    [SerializeField] private GridSystemVisual.GridVisualType gridVisualType;
    protected Unit unit;
    protected bool IsActive;
    private float CIRCLE_ADJUST_FACTOR = 0.3f;
   
    protected Animator animator; 

    protected Action OnActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        IsActive = false;
        animator = GetComponentInChildren<Animator>();
    }

    public string GetActionName() => nameOfAction;

    public virtual bool IsAttackAction() => false;

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
        Vector3 testPoint = LevelGrid.Instance.GetWorldPosition(resultGridPos);
        float testDistance = Vector3.Distance(testPoint, unit.GetWorldPosition());
        return Mathf.RoundToInt(testDistance-CIRCLE_ADJUST_FACTOR) > effectiveDistance * LevelGrid.Instance.GetCellSize();
    }

    /*public virtual bool APathDistance(List<GridPosition> path) {
        int sum = 0;
        foreach (var node in path)
        {
            sum += Mathf.Min(node)
        }
    }*/

    //private void OnDrawGizmosSelected() {
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0.1f, 0),effectiveDistance * LevelGrid.instance.GetCellSize());
   // }

    public virtual bool HandleUnitState() => true;
    public virtual string GenerateUnitStateErrorMessage() => "";
    public Unit GetUnit() => unit;

    public GridSystemVisual.GridVisualType GetGridVisualType() => gridVisualType;

    public EnemyAIAction GetBestEnemyAIAction() {
        List<EnemyAIAction> enemyAIActions = new List<EnemyAIAction>();
        List<GridPosition> validGridPositions = GetValidGridPositionList();
        foreach (GridPosition gridPosition in validGridPositions)
        {
            enemyAIActions.Add(GetEnemyAIAction(gridPosition));
        }
        if(enemyAIActions.Count == 0) {
            return null;
        }
        enemyAIActions.Sort((EnemyAIAction actionOne, EnemyAIAction actionTwo) => actionTwo.actionValue - actionOne.actionValue);
        return enemyAIActions[0];
    }

    private EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction{
            baseAction = this,
            gridPosition = gridPosition,
            actionValue = CalculateEnemyAIActionValue(),
        };
    }


    // override this to make your own AI Action Value.
    protected abstract int CalculateEnemyAIActionValue();
}
