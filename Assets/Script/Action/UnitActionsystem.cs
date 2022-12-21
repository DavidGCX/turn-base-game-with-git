using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UnitActionsystem : MonoBehaviour
{
    public static UnitActionsystem Instance {get; private set; }
    public event Action SelectEvent;

    public event Action OnSelectedActionChange;

    public event Action StartAction;
    public event Action FinishAction;

    public event Action UnitDeselect;

    [SerializeField] private LayerMask UnitySelectLayerMask;
    private Unit selectedUnit;

    private BaseAction selectedAction;

    private bool isBusy = false;

    private List<Unit> totalUnits;
    [SerializeField] private GameObject BusyUI;
    [SerializeField] private Transform Notification;

    [SerializeField] private Transform unitPrefab;

    [SerializeField] Unit testUnitOne;
    [SerializeField] Unit testUnitTwo;

    [SerializeField] Transform unitContainer;

    private void Awake() {
        Instance = this;
        totalUnits = new List<Unit>();
        if (testUnitOne != null) {
            totalUnits.Add(testUnitOne);
        }
        if (testUnitTwo != null) {
            totalUnits.Add(testUnitTwo);
        }
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
        TryHandleUnitSpawn();
        HandleSelectedAction();
        if(Input.GetKeyDown(KeyCode.N)) {
            LevelGrid.instance.CreateANewGridSystem(testUnitOne.transform);
        }
    }

    private void TryHandleUnitSpawn() {
        if (Input.GetMouseButtonDown(1)) {
            if (!LevelGrid.instance.HasAnyUnitOnGridPosition(new GridPosition(0, 0))) {
                Vector3 spawnPlace = LevelGrid.instance.GetWorldPosition(new GridPosition(0, 0));  
                Transform newUnit = Instantiate(unitPrefab, spawnPlace, Quaternion.identity, unitContainer);
                Unit spawnUnit = newUnit.GetComponent<Unit>();
                totalUnits.Add(spawnUnit);
                SetSelectedUnit(spawnUnit);
            } else {
                SendNotification("Grid has already occupied by a unit");
            }
        } 

        
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
        UnitDeselect?.Invoke();
        selectedUnit = unit;
        if (selectedUnit == null) {return;}
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
                SendNotification("Not enough action point");
                return;   
            }
            StartAction?.Invoke();
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
        FinishAction?.Invoke();
    }

    private void SendNotification(string words) {
        Notification.gameObject.SetActive(true);
        Notification.gameObject.GetComponent<TMP_Text>().text = words;
    }
}
