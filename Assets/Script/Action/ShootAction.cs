using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShootAction : BaseAction
{
    private enum State {
        Aiming,
        Shooting,
        Cooloff,
    }
    [SerializeField] private float aimTime = 0.5f;

    private State state;
    private bool stateComplete;

    private bool insideRoutine;
    private Unit TargetUnit;

    private bool canShoot;

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
                if(!insideRoutine) {
                    StartCoroutine("Aiming");
                }
                break;
            case State.Shooting:
                if(!insideRoutine) {
                    StartCoroutine("Shooting");
                }
                break;
            case State.Cooloff:
                if(!insideRoutine) {
                    StartCoroutine("CoolOff");
                }
                break;
        }
        if (stateComplete) {
            insideRoutine = false;
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
        //Debug.Log(state);
    }

    public IEnumerator Aiming() {
        insideRoutine = true;
        Vector3 rotateDirection = (TargetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
        Tween a = transform.DOLookAt(TargetUnit.GetWorldPosition(), aimTime);
        yield return a.WaitForCompletion();
        stateComplete = true;
    }

    public IEnumerator Shooting() {
        insideRoutine = true;
        TargetUnit.Damage();
        yield return new WaitForSeconds(.2f);
        stateComplete = true;
    }

     public IEnumerator CoolOff() {
        insideRoutine = true;
        yield return new WaitForSeconds(.5f);
        stateComplete = true;
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        state = State.Aiming;
        Debug.Log(state);
        insideRoutine = false;
        stateComplete = false;
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

    public override string GenerateUnitStateErrorMessage()
    {
        return "Still can not shoot, Check the status bar";
    }

    public override bool HandleUnitState(){
        if(unit.CheckStatus(Unit.UnitStatus.CanNotShoot)) {
            unit.RemoveStatus(Unit.UnitStatus.CanNotShoot);
            return false;
        } else {
            return true;
        }
    }
    
}
