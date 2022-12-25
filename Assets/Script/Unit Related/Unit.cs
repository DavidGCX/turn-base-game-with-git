using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private SpinAction spinAction;
    private MoveAction moveAction;

    [SerializeField]private int currentActionPoint = 5;

    [SerializeField] private int maximumActionPoint = 5;

    private BaseAction[] baseActions;
    private GridPosition lastGridPosition;

    public static event Action ActionPointChangeDueToTurnEnd;
   
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
        TurnSystem.instance.OnTurnChange += NewTurn;

    }
    
    void Update()
    {
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
    }



    /* public SpinAction GetSpinAction() {
        return spinAction;
    }*/
    

    public bool CanSpendActionPoint(BaseAction baseAction) {
        if (baseAction.GetActionSpent() > currentActionPoint) {
            return false;
        } else {
            return true;
        }
    }

    public bool TrySpendActionPoint(BaseAction baseAction) {
        if (CanSpendActionPoint(baseAction)) {
            currentActionPoint -= baseAction.GetActionSpent();
            return true;
        } else {
            return false;
        }
    }

    public void NewTurn() {
        currentActionPoint = maximumActionPoint;
        ActionPointChangeDueToTurnEnd?.Invoke();
    }


    public GridPosition GetGridPosition() => lastGridPosition;

    public Vector3 convertedPosition(Vector3 Mouse) => 
    LevelGrid.instance.GetWorldPosition(LevelGrid.instance.GetGridPosition(Mouse));

    public BaseAction[] GetBaseActions() => baseActions;
    public int GetCurrentActionPoint() => currentActionPoint;

    public int GetMaxActionPoint() => maximumActionPoint;
    public MoveAction GetMoveAction() => moveAction;
}

