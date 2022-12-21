using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private SpinAction spinAction;
    private MoveAction moveAction;

    private int actionPoint = 2;

    private BaseAction[] baseActions;
    private GridPosition lastGridPosition;
   
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActions = GetComponents<BaseAction>();
    }
    
    
    private void Start()
    {
        GridPosition gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        lastGridPosition = gridPosition;
        LevelGrid.instance.AddUnitAtGridPosition(gridPosition, this);
    }
    
    void Update()
    {
        GridPosition gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        if (gridPosition.x < 0) {
            gridPosition.x = 0;
             Debug.Log($"Out of Boundary, current pos is {LevelGrid.instance.GetGridPosition(transform.position)}");
        } 
        if (gridPosition.z < 0) {
            Debug.Log($"Out of Boundary, current pos is {LevelGrid.instance.GetGridPosition(transform.position)}");
            gridPosition.z = 0;
        }
        if (lastGridPosition != gridPosition) {
            LevelGrid.instance.UnitMoveGridPosition(this, lastGridPosition, gridPosition);
            lastGridPosition = gridPosition;
        }
        /* if (Input.GetMouseButtonDown(0)) {
            Move(MouseWorld.GetMousePosition());
        } */
    }

    public MoveAction GetMoveAction() {
        return moveAction;
    }

    /* public SpinAction GetSpinAction() {
        return spinAction;
    }*/
    
    public GridPosition GetGridPosition() => lastGridPosition;

    public Vector3 convertedPosition(Vector3 Mouse) => 
    LevelGrid.instance.GetWorldPosition(LevelGrid.instance.GetGridPosition(Mouse));

    public BaseAction[] GetBaseActions() => baseActions;

    public bool CanSpendActionPoint(BaseAction baseAction) {
        if (baseAction.GetActionSpent() >= actionPoint) {
            return false;
        } else {
            return true;
        }
    }

    public bool TrySpendActionPoint(BaseAction baseAction) {
        if (CanSpendActionPoint(baseAction)) {
            actionPoint -= baseAction.GetActionSpent();
            return true;
        } else {
            return false;
        }
    }
}

