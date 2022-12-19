using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionsystem : MonoBehaviour
{
    public static UnitActionsystem Instance {get; private set; }
    public event Action SelectEvent;
    [SerializeField] private LayerMask UnitySelectLayerMask;
    [SerializeField] Unit selectedUnit;

    private bool isBusy = false;
    private void Awake() {
        Instance = this;
    }

    void Update()
    {
        if(isBusy == true) {
            return;
        }
        if (Input.GetMouseButtonDown(0)) {
            if (TryHandleUnitSelection()) {
                return;
            } else {
                SetBusy();
                GridPosition convertedPosition = LevelGrid.instance.GetGridPosition(MouseWorld.GetMousePosition());
                if(selectedUnit.GetMoveAction().IsValidMoveGridPosition(convertedPosition))
                selectedUnit.GetMoveAction().Move(convertedPosition, ClearBusy);
            } 
        }
        if (Input.GetMouseButtonDown(1)) {
            SetBusy();
            selectedUnit.GetSpinAction().Spin(ClearBusy);
        } 
    }

    private bool TryHandleUnitSelection() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, UnitySelectLayerMask)) {
            if(rayCastHit.transform.TryGetComponent<Unit>(out Unit unit)) {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit) {
        selectedUnit = unit;
        SelectEvent?.Invoke();
    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }

    private void SetBusy() {
        isBusy = true;
    }

    private void ClearBusy() {
        isBusy = false;
    }
}
