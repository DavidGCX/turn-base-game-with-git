using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackAction : BaseAction
{
    private enum State {
        Aiming,
        Attacking,
        Cooloff,
    }
    [SerializeField] private float aimTime = 0.5f;

    private State state;
    private bool stateComplete;

    private bool insideRoutine;
    private Unit TargetUnit;
    [SerializeField] private int BaseWeaponDamage = 20;
    [SerializeField] private int ApWeaponDamage = 10;
    [SerializeField] private float DamageRandomRate = 20f;
    private bool canShoot;

    protected override void Awake() {
        base.Awake();
        nameOfAction = "Attack";
    }
    private void Update()
    {
        if(!IsActive) {
            return;
        }
        //start each Coroutine depends on the state
        switch (state)
        {
            case State.Aiming:
                if(!insideRoutine) {
                    StartCoroutine("Aiming");
                }
                break;
            case State.Attacking:
                if(!insideRoutine) {
                    StartCoroutine("Attacking");
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

    //State change
    private void NextState() {
        switch (state)
        {
            case State.Aiming:
                state = State.Attacking;
                break;
            case State.Attacking:
                state = State.Cooloff;
                break;
            case State.Cooloff:
                EndAction();
                break;
        }
        //Debug.Log(state);
    }

    // Turn to the target
    public IEnumerator Aiming() {
        insideRoutine = true;
        Vector3 rotateDirection = (TargetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
        Tween a = transform.DOLookAt(TargetUnit.GetWorldPosition(), aimTime);
        yield return a.WaitForCompletion();
        stateComplete = true;
    }

    // Attacking animation and calculation need to go here and override
    protected virtual IEnumerator Attacking() {
        insideRoutine = true;
        TargetUnit.Damage(BaseWeaponDamage, ApWeaponDamage, unit.GetUnitAttackTotal(), DamageRandomRate);
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

    //Print a string for printing message
    public override string GenerateUnitStateErrorMessage()
    {
        return "Still can not shoot, Check the status bar";
    }

    //Check if can shoot or not
    public override bool HandleUnitState(){
        if(unit.CheckStatus(UnitStatsAndStatus.CurrentStatus.CanNotShoot)) {
            unit.RemoveStatus(UnitStatsAndStatus.CurrentStatus.CanNotShoot);
            return false;
        } else {
            return true;
        }
    }

    public override bool IsAttackAction() => true;
}