using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    public delegate void SpinDelegate();

    private void Start() {
        name = "Spin";
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
            IsActive = false;
            OnActionComplete();
        }
    }

    public void Spin(Action onActionComplete) {
        
    }

    public override string GetActionName() => "Spin"; 

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.OnActionComplete = onActionComplete;
        IsActive = true;
        totalSpinAmount = 0f;
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>{unitGridPosition};
    }
}
