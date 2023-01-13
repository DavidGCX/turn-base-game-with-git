using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{

    protected override void Awake() {
        base.Awake();
    }
    private float totalSpinAmount;
    private void Update()
    {
        if(!IsActive) {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f)
        {
            EndAction();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        StartAction(onActionComplete);
        totalSpinAmount = 0f;
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>{unitGridPosition};
    }

    
    protected override int CalculateEnemyAIActionValue() => 0;
}
