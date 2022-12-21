using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionsystem : MonoBehaviour
{
    public static UnitActionsystem Instance {get; private set; }
    public event Action SelectEvent;

    public event Action OnSelectedActionChange;

    [SerializeField] private LayerMask UnitySelectLayerMask;
    private Unit selectedUnit;

    private BaseAction selectedAction;

    private bool isBusy = false;

    [SerializeField] private GameObject BusyUI;
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        //SetSelectedUnit(selectedUnit);
    }
    private void Update()
    {
        if(isBusy == true || 
        TryHandleUnitSelection() ||
        EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        HandleSelectedAction();

    }

    private bool TryHandleUnitSelection() {

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, UnitySelectLayerMask)) {
                if(rayCastHit.transform.TryGetComponent<Unit>(out Unit unit)) {
                    if(unit == selectedUnit) {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit) {
        selectedUnit = unit;
        SetSelectedAction(selectedUnit.GetMoveAction());
        SelectEvent?.Invoke();
    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }

    public void SetSelectedAction(BaseAction baseAction) {
        if(!isBusy) {
            selectedAction = baseAction;
        }
        OnSelectedActionChange.Invoke();
        
    }

    public BaseAction GetSelectedAction() => selectedAction;

    private void HandleSelectedAction() {
        if(selectedAction == null) return;
        if(Input.GetMouseButtonDown(0)) {
            GridPosition convertedPosition = LevelGrid.instance.GetGridPosition(MouseWorld.GetMousePosition());
            if(!selectedAction.IsValidMoveGridPosition(convertedPosition)) {
                return;
            }
            if(!selectedUnit.TrySpendActionPoint(selectedAction)){
                Debug.Log(selectedUnit.GetCurrentActionPoint());
                return;   
            }
            SetBusy();
            selectedAction.TakeAction(convertedPosition, ClearBusy);
        }
    }

    private void SetBusy() {
        isBusy = true;
        BusyUI.SetActive(true);
    }

    private void ClearBusy() {
        isBusy = false;
        SetSelectedAction(selectedUnit.GetMoveAction());
        BusyUI.SetActive(false);
    }


}
