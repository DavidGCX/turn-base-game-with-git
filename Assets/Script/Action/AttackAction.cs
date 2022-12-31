using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackAction : BaseAction
{

    /*Do not use this script directly, use this as the base class for attacking,
    You should override or set the following to make an actual attack:
    (以下请在面板（inspector）修改)

    nameOfAction (the name of that attack) 

    effectiveDistance  (will be like a circle, check it in play mode or use Gizmoz to draw a cirle to view the idea)
                        (The actual implementation is in the BaseAction Class, check it for detail or change it)

    actionPointRequirement (Action point required)

    BaseWeaponDamage (Base weapon damage)

    ApWeaponDamage (Armor penetration damage)

    DamageRandomRate (伤害随机上下浮动 DamageRandomRate%) (只有这个是浮点数float)

    (以下请写在脚本里修改)
    
    IEnumerator Attacking() 
        Include all attack animation, attack effect here. You need to call targetunit.damage inside.
        所有攻击的东西都放在这里处理
    
    How to calculate the damage （伤害是怎么计算的)    
    Total Damage = (ApWeaponDamage + BaseWeaponDamage * (1 - {The Armor of Unit} / 200)) * (1 + Random(-DamageRandomRate, DamageRandomRate)) 


    Animator has been included in the BaseAction class, you should use the one attached to the unit and add animation there.
     
    ***********************************************************************************************************************************
        *****for all stats change it in the inspector (去面板改变单位属性, 包括单位攻击防御武器威力等，不要在脚本中直接修改，会难以debug)*****
    ***********************************************************************************************************************************
    */
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

    // Attacking animation and calculation need to go here and override. below is an example
    protected virtual IEnumerator Attacking() {
        insideRoutine = true;

        //Can play animation like this:
        // animator.Play("firing rifle");

        // Causing Damage like this:
        //TargetUnit.Damage(BaseWeaponDamage, ApWeaponDamage, unit.GetUnitAttackTotal(), DamageRandomRate);
        
        // Use to wait for specific time, 0.2f in the below example
        yield return new WaitForSeconds(.2f);
        /*
        animator.Play("reloading");
        yield return new WaitForSeconds(3f);
        Optional reloading animation
        */ 
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
