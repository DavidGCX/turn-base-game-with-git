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
    private void Awake() {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (TryHandleUnitSelection()) {
                return;
            } else {
                GridPosition convertedPosition = LevelGrid.instance.GetGridPosition( MouseWorld.GetMousePosition());
                if(selectedUnit.GetMoveAction().IsValidMoveGridPosition(convertedPosition))
                selectedUnit.GetMoveAction().Move(convertedPosition);
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
        SelectEvent?.Invoke();
    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }
}
