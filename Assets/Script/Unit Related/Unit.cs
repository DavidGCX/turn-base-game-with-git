using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    private SpinAction spinAction;
    private MoveAction moveAction;

    [SerializeField]private int currentActionPoint = 5;

    [SerializeField] private int maximumActionPoint = 5;
    [SerializeField] private Transform ActionPointContainer;

    [SerializeField] private Transform ActionPointReadyPrefab;
    [SerializeField] private Transform ActionPointSelectedPrefab;

    [SerializeField] private Transform ActionPointUsedPrefab;

    [SerializeField] private TMP_Text ActionPointCount;

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
        HandleActionPoint();
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

    public void HandleActionPoint() {
        if (UnitActionsystem.Instance.GetSelectedUnit() != this || 
        UnitActionsystem.Instance.GetSelectedAction() == null){
            UpdateActionPoint(0);
        } else{
            UpdateActionPoint(UnitActionsystem.Instance.GetSelectedAction().GetActionSpent());
        }
        ActionPointCount.text = $"{currentActionPoint}/{maximumActionPoint}";
    }

    public void UpdateActionPoint(int selectedAmount) {
        ClearPanel();
        int max = maximumActionPoint;
        int current = currentActionPoint - selectedAmount;
        int used = max - current - selectedAmount;
        //Debug.Log(used);
        if(current >= 0) {
            for (int i = 0; i < selectedAmount; i++)
            {
                Instantiate(ActionPointSelectedPrefab,ActionPointContainer);
            }
            for (int j = 0; j < current; j++)
            {
                Instantiate(ActionPointReadyPrefab,ActionPointContainer);
            }
            for (int k = 0; k < used; k++)
            {
                Instantiate(ActionPointUsedPrefab,ActionPointContainer);
            }
        } else {
            current += selectedAmount;
            for (int j = 0; j < current; j++)
            {
                Instantiate(ActionPointReadyPrefab,ActionPointContainer);
            }
            for (int i = 0; i < max - current; i++)
            {
                Instantiate(ActionPointUsedPrefab,ActionPointContainer);
            }
        }
    }

    public void ClearPanel() {
        //Debug.Log("This one is called");
        foreach (Transform item in ActionPointContainer)
        {
            Destroy(item.gameObject);
        }
    }

    public GridPosition GetGridPosition() => lastGridPosition;

    public Vector3 convertedPosition(Vector3 Mouse) => 
    LevelGrid.instance.GetWorldPosition(LevelGrid.instance.GetGridPosition(Mouse));

    public BaseAction[] GetBaseActions() => baseActions;
    public int GetCurrentActionPoint() => currentActionPoint;

    public int GetMaxActionPoint() => maximumActionPoint;
    public MoveAction GetMoveAction() => moveAction;
}

