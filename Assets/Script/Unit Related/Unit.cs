using System.Diagnostics;
using System.Reflection;
using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    private MoveAction moveAction;

    private UnitStatsAndStatus unitStatsAndStatus;
    private UnitWorldUI unitWorldUI;

    private BaseAction[] baseActions;
    private GridPosition lastGridPosition;

   
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        baseActions = GetComponents<BaseAction>();
        unitStatsAndStatus = GetComponent<UnitStatsAndStatus>();
        unitWorldUI = GetComponent<UnitWorldUI>();
    }
    
    
    private void Start()
    {
        GridPosition gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        lastGridPosition = gridPosition;
        LevelGrid.instance.AddUnitAtGridPosition(gridPosition, this);
    }
    
    void Update()
    {
        //Set the gridPosition for the unit;
        GridPosition gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        if (gridPosition.x < 0) {
            gridPosition.x = 0;
            // Debug.Log($"Out of Boundary, current pos is {LevelGrid.instance.GetGridPosition(transform.position)}");
        } 
        if (gridPosition.z < 0) {
            //Debug.Log($"Out of Boundary, current pos is {LevelGrid.instance.GetGridPosition(transform.position)}");
            gridPosition.z = 0;
        }
        if (lastGridPosition != gridPosition) {
            LevelGrid.instance.UnitMoveGridPosition(this, lastGridPosition, gridPosition);
            lastGridPosition = gridPosition;
        }
        HandleActionPoint();
    }



    
    // Action Point Related:
    public bool CanSpendActionPoint(BaseAction baseAction) => unitStatsAndStatus.CanSpendActionPoint(baseAction);

    public bool TrySpendActionPoint(BaseAction baseAction) => unitStatsAndStatus.TrySpendActionPoint(baseAction);

    public void HandleActionPoint() {
        unitWorldUI.HandleActionPoint();
    }



    // See UnitStatsAndStatus.cs
    public void Damage(int baseDamage, int apDamage, int totalAttack, float damageRandomRate) {
        unitStatsAndStatus.Damage(baseDamage, apDamage, totalAttack, damageRandomRate);
    }



    public void ClearStatus() {
        unitStatsAndStatus.ClearStatus();
    }

    public void AddStatus(UnitStatsAndStatus.CurrentStatus unitStatus) {
        unitStatsAndStatus.AddStatus(unitStatus);
    }

    public void RemoveStatus(UnitStatsAndStatus.CurrentStatus unitStatus) {
        unitStatsAndStatus.RemoveStatus(unitStatus);
    }

    public void SetStats(int amount, UnitStatsAndStatus.Stats stat){
        unitStatsAndStatus.SetStats(amount, stat);
    }

    public int GetStats(UnitStatsAndStatus.Stats stat) => unitStatsAndStatus.GetStats(stat);
  
    public bool CheckStatus(UnitStatsAndStatus.CurrentStatus unitStatus) => unitStatsAndStatus.CheckStatus(unitStatus);

    public GridPosition GetGridPosition() => lastGridPosition;

     public Vector3 GetWorldPosition() => LevelGrid.instance.GetWorldPosition(lastGridPosition);


    public BaseAction[] GetBaseActions() => baseActions;
    public int GetCurrentActionPoint() => unitStatsAndStatus.GetCurrentActionPoint();

    public int GetMaxActionPoint() => unitStatsAndStatus.GetMaxActionPoint();
    public MoveAction GetMoveAction() => moveAction;

    public bool GetUnitType() => unitStatsAndStatus.GetUnitType();
    public void SetUnitType(bool type) {unitStatsAndStatus.SetUnitType(type);}

    public int GetUnitAttackTotal() => unitStatsAndStatus.GetUnitAttackTotal();
    public int GetUnitAttackBase() => unitStatsAndStatus.GetUnitAttackBase();
}

