using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private MoveAction moveAction;

    private GridPosition lastGridPosition;
   
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
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
    
    public GridPosition GetGridPosition() => lastGridPosition;
}
