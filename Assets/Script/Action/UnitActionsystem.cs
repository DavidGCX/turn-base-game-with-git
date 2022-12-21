using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionsystem : MonoBehaviour
{
    public static UnitActionsystem Instance {get; private set; }
    public event Action SelectEvent;
    [SerializeField] private LayerMask UnitySelectLayerMask;
    [SerializeField] private Unit selectedUnit;

    private BaseAction selectedAction;

    private bool isBusy = false;

    [SerializeField] private GameObject BusyUI;
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        SetSelectedUnit(selectedUnit);
    }
    private void Update()
    {
        if(isBusy == true) {
            return;
        }
        if (Input.GetMouseButtonDown(0)) {
            if (TryHandleUnitSelection()) {
                return;
            } else {
                HandleSelectedAction();
            }
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
        SetSelectedAction(unit.GetMoveAction());
        SelectEvent?.Invoke();
    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }

    public void SetSelectedAction(BaseAction baseAction) {
        if(!isBusy) {
            selectedAction = baseAction;
        }
        
    }

    public BaseAction GetSelectedAction() => selectedAction;

    private void HandleSelectedAction() {
        if(Input.GetMouseButtonDown(0)) {
            GridPosition convertedPosition = LevelGrid.instance.GetGridPosition(MouseWorld.GetMousePosition());
            if(selectedAction.IsValidMoveGridPosition(convertedPosition)) {
                SetBusy();
                selectedAction.TakeAction(convertedPosition, ClearBusy);
            }
        }
    }

    private void SetBusy() {
        isBusy = true;
        BusyUI.SetActive(true);
    }

    private void ClearBusy() {
        isBusy = false;
        BusyUI.SetActive(false);
    }


}
