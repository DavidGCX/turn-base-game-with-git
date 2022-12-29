using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private enum State {
        Aiming,
        Shooting,
        Cooloff,
    }

    private State state;
    private bool stateComplete;
    private Unit TargetUnit;

     protected override void Awake() {
        base.Awake();
        nameOfAction = "Shoot";
        actionPointRequirement = 1;
    }
    private void Update()
    {
        if(!IsActive) {
            return;
        }

        switch (state)
        {
            case State.Aiming:
            stateComplete = true;
                break;
            case State.Shooting:
            stateComplete = true;
                break;
            case State.Cooloff:
                break;
        }
        if (stateComplete) {
            stateComplete = false;
            NextState();
        }
        
    } 

    private void NextState() {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                break;
            case State.Shooting:
                state = State.Cooloff;
                break;
            case State.Cooloff:
                EndAction();
                break;
        }
        Debug.Log(state);
    }

    public IEnumerator Aiming() {
        yield return RotateToTarget();
    }

    public IEnumerator RotateToTarget(){
        Vector3 rotateDirection = (TargetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
        //float rotate speed = 
        yield return null;
    } 
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        state = State.Aiming;
        Debug.Log(state);
        TargetUnit = LevelGrid.instance.GetUnitAtGridPosition(gridPosition);
        StartAction(onActionComplete);

    }

    //Show grid in attack range but not the target grid;
    public List<GridPosition> GetTargetGridPositionList()
    {
                List<GridPosition> validGridPositionList = new List<GridPosition>();
        
        for (int i = -effectiveDistance; i <=effectiveDistance; i++) {
            for (int j = -effectiveDistance; j <=+effectiveDistance; j++)
            {
                GridPosition offsetGridpos = new GridPosition(i,j);
                GridPosition resultGridpos = unit.GetGridPosition() + offsetGridpos;
                if (!LevelGrid.instance.IsAValidGridPosition(resultGridpos)) {
                    continue;
                }
                if(InvalidDistance(resultGridpos) && effectiveDistance != 1) {
                    // Not in distance
                    continue;
                }
                if (LevelGrid.instance.HasAnyUnitOnGridPosition(resultGridpos)) {
                    //has unit
                    continue;
                }
                validGridPositionList.Add(resultGridpos);
            }
        }
        /* foreach (var item in validGridPositionList)
        {
            Debug.Log(item);
        } */
        
        return validGridPositionList;
    }

    // show grid that can be used to attack;
    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        for (int i = -effectiveDistance; i <=effectiveDistance; i++) {
            for (int j = -effectiveDistance; j <=+effectiveDistance; j++)
            {
                GridPosition offsetGridpos = new GridPosition(i,j);
                GridPosition resultGridpos = unit.GetGridPosition() + offsetGridpos;
                if (!LevelGrid.instance.IsAValidGridPosition(resultGridpos)) {
                    continue;
                }
                if(InvalidDistance(resultGridpos) && effectiveDistance != 1) {
                    continue;
                }
                if (!LevelGrid.instance.HasAnyUnitOnGridPosition(resultGridpos)) {
                    continue;
                }
                Unit targetUnit = LevelGrid.instance.GetUnitAtGridPosition(resultGridpos);
                if(targetUnit.GetUnitType() == unit.GetUnitType()) {
                    continue;
                }
                validGridPositionList.Add(resultGridpos);
            }
        }
        return validGridPositionList;
    }
}
