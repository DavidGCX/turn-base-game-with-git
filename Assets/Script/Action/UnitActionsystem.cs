using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance {get; private set; }
    public event Action SelectEvent;

    public event Action OnSelectedActionChange;
    
    [SerializeField] private LayerMask UnitySelectLayerMask;
    private Unit selectedUnit;

    private BaseAction selectedAction;

    private bool isBusy = false;



    // for traking the units, could be use in the future
    private List<Unit> totalPlayerUnits;
    private List<Unit> totalEnemyUnits;


    [SerializeField] private GameObject BusyUI;
    [SerializeField] private Transform Notification;

    [SerializeField] private Transform unitPrefab;


    // current unit in the plane, for testing purpose, can be added or removed;
    [SerializeField] Unit testUnitOne;
    [SerializeField] Unit testUnitTwo;

    // for storing the unit, no actual use
    [SerializeField] Transform unitContainer;

    private void Awake() {
        Instance = this;
        totalPlayerUnits = new List<Unit>();
        totalEnemyUnits = new List<Unit>();
        if (testUnitOne != null) {
            totalPlayerUnits.Add(testUnitOne);
        }
        if (testUnitTwo != null) {
            totalEnemyUnits.Add(testUnitTwo);
        }
    }

    private void Start() {
        //SetSelectedUnit(selectedUnit);
        TurnSystem.instance.OnTurnChange += TurnChange;
    }
    private void Update()
    {
        if(!TurnSystem.instance.IsPlayerTurn()) {
            return;
        }
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


    // Used to generate unit
    private void TryHandleUnitSpawn() {
        if (Input.GetMouseButtonDown(1)) {
            if (!LevelGrid.instance.HasAnyUnitOnGridPosition(new GridPosition(0, 0))) {
                Vector3 spawnPlace = LevelGrid.instance.GetWorldPosition(new GridPosition(0, 0));  
                Transform newUnit = Instantiate(unitPrefab, spawnPlace, Quaternion.identity, unitContainer);
                Unit spawnUnit = newUnit.GetComponent<Unit>();
                totalPlayerUnits.Add(spawnUnit);
                SetSelectedUnit(spawnUnit);
            } else {
                SendNotification("Grid has already occupied by a unit");
            }
        } 

        
    }


    // check if the mouse click on the unselected unit.
    private bool TryHandleUnitSelection() {

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, UnitySelectLayerMask)) {
                if(rayCastHit.transform.TryGetComponent<Unit>(out Unit unit)) {
                    if(unit == selectedUnit) {
                        CameraController.Instance.FocusOnWorldPositon(selectedUnit.GetWorldPosition());
                        return false;
                    }
                    if(unit.GetUnitType()) {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }


    // As the name described, select the passed in unit
    private void SetSelectedUnit(Unit unit) {
        selectedUnit = unit;
        if (selectedUnit == null) {UnitActionSystemUI.Instance.DestroyAllButton();return;}
        SetSelectedAction(selectedUnit.GetMoveAction());
        SelectEvent?.Invoke();
        CameraController.Instance.FocusOnWorldPositon(selectedUnit.GetWorldPosition());
    }


    // As the name described, select the passed in action
    public void SetSelectedAction(BaseAction baseAction) {
        if(!isBusy) {
            selectedAction = baseAction;
        }
        OnSelectedActionChange.Invoke();
        
    }
    
    // Trigger the acion on valid grid depends on the each action's setting, check each action for 
    // how to get the valid grid position
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
            if(!selectedAction.HandleUnitState()) {
                SendNotification(selectedAction.GenerateUnitStateErrorMessage());
                return;
            }
            SetBusy();
            selectedAction.TakeAction(convertedPosition, ClearBusy);
        }
    }


    // use to trigger the busy ui;
    private void SetBusy() {
        isBusy = true;
        BusyUI.SetActive(true);
    }


    // use to hide busy ui, when action point is not enough for moving, set the selected action to null
    private void ClearBusy() {
        isBusy = false;
        //SetSelectedAction(selectedUnit.GetMoveAction());
        if(!selectedUnit.CanSpendActionPoint(selectedAction)){
            SetSelectedAction(null); 
        }
        BusyUI.SetActive(false);
    }


    // Send am actual notification using the bottom notification window, check this in play mode
    public void SendNotification(string words) {
        Notification.gameObject.SetActive(true);
        Notification.gameObject.GetComponent<TMP_Text>().text = words;
    }


    // just to clear the selected unit when turn change, would not affect the selected action since the handle unit selection
    // could deal with it to avoid problem
    public void TurnChange() {
        SetSelectedUnit(null);
    }



    public Unit GetSelectedUnit() => selectedUnit;
    public BaseAction GetSelectedAction() => selectedAction;



    // No actual use for now, just to keep track of the units on the place
    public List<Unit> GetTotalUnitList() => add(totalPlayerUnits, totalEnemyUnits);
    public List<Unit> GetEnemyUnitList() => totalEnemyUnits;
    public List<Unit> GetUnitList() => totalPlayerUnits;
    public void RemoveUnitFromList(Unit unit, bool isEmemy) {
        if(isEmemy) {
            totalEnemyUnits.Remove(unit);
        } else {
            totalPlayerUnits.Remove(unit);
        }
    } 
    public List<Unit> add(List<Unit> a, List<Unit> b) {
        List<Unit> c = new List<Unit>();
        foreach (var item in a)
        {
            c.Add(item);
        }
        foreach (var item in b)
        {
            c.Add(item);
        }
        return c;
        
    }
}
