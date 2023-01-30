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

    effectiveDistance  (will be like a circle, check it in play mode or use Gizmos to draw a circle to view the idea)
                        (The actual implementation is in the BaseAction Class, check it for detail or change it)

    actionPointRequirement (Action point required)

    BaseWeaponDamage (Base weapon damage)

    ApWeaponDamage (Armor penetration damage)

    gridVisualTypeForRange (The color for grid in range without target, 设置在范围内但无目标的格子的颜色)

    DamageRandomRate (伤害随机上下浮动 DamageRandomRate%) (只有这个是浮点数float)

    (以下请写在脚本里修改)
    
    IEnumerator SpecificAttack() 
        Include all attack animation, attack effect here. You need to call targetUnit.damage inside.
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
    protected Unit targetUnit;

    //protected Unit selfUnit;
    [Header("武器威力部分")]
    [Tooltip("基础武器威力，会被护甲减免")]
    [SerializeField] private int BaseWeaponDamage = 20;
    [Tooltip("护甲穿透武器威力，不会被护甲减免")]
    [SerializeField] private int ApWeaponDamage = 10;
    [Tooltip("武器威力总和上下浮动百分比，计算时为：武器威力计算护甲减免后 * random（1 - 浮动率， 1 + 浮动率）")]
    [SerializeField] private float DamageRandomRate = 20f;

    [Header("攻击动作特写镜头位置")]
    [SerializeField] protected Transform attackCameraPosition;

     [Header("攻击范围的格子颜色（攻击目标以外的位置）")]
     [Tooltip("在GridVisualController的列表中添加更多选项")]
    [SerializeField] protected GridSystemVisual.GridVisualType gridVisualTypeForRange;

    public static event EventHandler<AttackActionCameraArgs> OnAttackActionCameraRequired;

    public static event Action OnAttackComplete;

    public class AttackActionCameraArgs : EventArgs {
        public Vector3 cameraPosition;
        public Vector3 cameraLookAtPosition;
        public AttackActionCameraArgs(Vector3 cameraPosition, Vector3 cameraLookAtPosition) {
            this.cameraPosition = cameraPosition;
            this.cameraLookAtPosition = cameraLookAtPosition;
        }
    }

    protected override void Awake() {
        base.Awake();
        //selfUnit = unit;
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
    private IEnumerator Aiming() {
        insideRoutine = true;
        Vector3 rotateDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
        Tween a = transform.DOLookAt(targetUnit.GetWorldPosition(), aimTime);
        yield return a.WaitForCompletion();
        stateComplete = true;
    }

    
    private IEnumerator Attacking() {
        insideRoutine = true;
        if(ShouldUseAttackCamera()) {
            UseAttackCamera();
        }
        const float CameraDefaultBlendTime = 0.2f;
        yield return new WaitForSeconds(CameraDefaultBlendTime * 2);
        yield return SpecificAttack();
         
        stateComplete = true;
    }


// Attacking animation and calculation need to go here and override. below is an example
    protected virtual IEnumerator SpecificAttack() {
        //Can play animation like this:
        animator.Play("firing rifle");
        // Causing Damage like this:
        
        //CauseDamage();

        // Use to wait for specific time, 0.2f in the below example
        yield return new WaitForSeconds(.2f);
        
        //Optional reloading animation
        animator.Play("reloading");
        yield return new WaitForSeconds(3f);
    }


    private IEnumerator CoolOff() {
       insideRoutine = true;
       BackToUsualCamera(); 
       yield return new WaitForSeconds(.3f);
       stateComplete = true;
    }


    // Set the attack camera Position and look at angle, can be override with other version
    protected virtual void UseAttackCamera() {
        Vector3 height =new Vector3(0, attackCameraPosition.position.y, 0);
        OnAttackActionCameraRequired?.Invoke(this, new AttackActionCameraArgs(attackCameraPosition.position, 
        targetUnit.GetWorldPosition() + height));
    }

    // Can be override to decide the conditions for triggering the attack camera
    protected virtual bool ShouldUseAttackCamera() => true;
    // Back to the usual camera
    protected void BackToUsualCamera() {
        OnAttackComplete?.Invoke();
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        state = State.Aiming;
        //Debug.Log(state);
        insideRoutine = false;
        stateComplete = false;
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        StartAction(onActionComplete);
    }

    //Show grid in attack range but not the target grid;
    public virtual List<GridPosition> GetGridPositionListInRange()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        for (int i = -effectiveDistance; i <=effectiveDistance; i++) {
            for (int j = -effectiveDistance; j <=+effectiveDistance; j++)
            {
                GridPosition offsetGridpos = new GridPosition(i,j);
                GridPosition resultGridpos = unit.GetGridPosition() + offsetGridpos;
                if (!LevelGrid.Instance.IsAValidGridPosition(resultGridpos)) {
                    continue;
                }
                if(InvalidDistance(resultGridpos) && effectiveDistance != 1) {
                    // Not in distance
                    continue;
                }
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(resultGridpos)) {
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
                if (!LevelGrid.Instance.IsAValidGridPosition(resultGridpos)) {
                    continue;
                }
                if(InvalidDistance(resultGridpos) && effectiveDistance != 1) {
                    continue;
                }
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(resultGridpos)) {
                    continue;
                }
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(resultGridpos);
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

    //Check if can shoot or not, you can write like this to use the status system;
    public override bool HandleUnitState(){
        if(unit.CheckStatus(UnitStatsAndStatusBase.CurrentStatus.CanNotShoot)) {
            unit.RemoveStatus(UnitStatsAndStatusBase.CurrentStatus.CanNotShoot);
            return false;
        } else {
            return true;
        }
    }

    public bool CauseDamage() {
        return targetUnit.Damage(BaseWeaponDamage, ApWeaponDamage, unit.GetUnitAttackTotal(), DamageRandomRate);
    }

    public bool CauseDamage(Unit actualHitUnit) {
         return actualHitUnit.Damage(BaseWeaponDamage, ApWeaponDamage, unit.GetUnitAttackTotal(), DamageRandomRate);
    }

    public override bool IsAttackAction() => true;

    public Unit GetTargetUnit() => targetUnit;

    public int GetTotalDamage() => BaseWeaponDamage + ApWeaponDamage;

    public GridSystemVisual.GridVisualType GetGridVisualTypeForRange() => gridVisualTypeForRange;

    protected override int CalculateEnemyAIActionValue() => 100;
}
