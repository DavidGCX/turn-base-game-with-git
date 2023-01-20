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

    public event Action OnTakeAction;

    public event Action OnEnemyDestroy;
    
    [SerializeField] private LayerMask UnitySelectLayerMask;
    private Unit selectedUnit;

    private BaseAction selectedAction;

    public event Action OnBusyChange;
    private bool isBusy = false;



    // for tracking the units, could be use in the future
    [SerializeField] private List<Unit> totalPlayerUnits;
    [SerializeField] private List<Unit> totalEnemyUnits;

    [SerializeField] private GameObject BusyUI;
    [SerializeField] private Transform Notification;

    [SerializeField] private Transform unitPrefab;


    // for storing the unit, no actual use
    [SerializeField] Transform unitContainer;

    private void Awake() {
        Instance = this;
        totalPlayerUnits = new List<Unit>();
        totalEnemyUnits = new List<Unit>();
    }

    private void Start() {
        //SetSelectedUnit(selectedUnit);
        TurnSystem.Instance.OnTurnChange += TurnChange;
    }
    private void Update()
    {
        if(!TurnSystem.Instance.IsPlayerTurn()) {
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
            LevelGrid.Instance.CreateANewGridSystem(LevelGrid.Instance.transform);
        }
    }


    // Used to generate unit, will be improved in the future
    private void TryHandleUnitSpawn() {
        if (Input.GetMouseButtonDown(1)) {
            if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(new GridPosition(0, 0))) {
                Vector3 spawnPlace = LevelGrid.Instance.GetWorldPosition(new GridPosition(0, 0));  
                Transform newUnit = Instantiate(unitPrefab, spawnPlace, Quaternion.identity, unitContainer);
                Unit spawnUnit = newUnit.GetComponent<Unit>();
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
    
    // Trigger the action on valid grid depends on the each action's setting, check each action for 
    // how to get the valid grid position
    private void HandleSelectedAction() {
        if(selectedAction == null) return;
        if(Input.GetMouseButtonDown(0)) {
            GridPosition convertedPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMousePosition());
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
            
            OnTakeAction?.Invoke();
            selectedAction.TakeAction(convertedPosition, ClearBusy);
            SetBusy();
        }
    }


    // use to trigger the busy ui;
    private void SetBusy() {
        isBusy = true;
        BusyUI.SetActive(true);
        selectedUnit.UpdateActionPoint(0);
        OnBusyChange?.Invoke();
    }
    

    // use to hide busy ui, when action point is not enough for moving, set the selected action to null
    private void ClearBusy() {
        isBusy = false;
        //SetSelectedAction(selectedUnit.GetMoveAction());
        
        if(!selectedUnit.CanSpendActionPoint(selectedAction)){
            SetSelectedAction(null); 
        } else {
            SetSelectedAction(selectedAction); 
        }
        BusyUI.SetActive(false);
        OnBusyChange?.Invoke();
    }


    // Send an actual notification using the bottom notification window, check this in play mode
    public void SendNotification(string words) {
        Notification.gameObject.SetActive(true);
        Notification.gameObject.GetComponent<TMP_Text>().text = words;
    }


    // just to clear the selected unit when turn change, would not affect the selected action since that will be handled through unit selection
    // could deal with it to avoid problem
    private void TurnChange() {
        SetSelectedUnit(null);
        SetSelectedAction(null);
    }

    public bool GetBusyStatus() => isBusy;

    public Unit GetSelectedUnit() => selectedUnit;
    public BaseAction GetSelectedAction() => selectedAction;



    // No actual use for now, just to keep track of the units on the place
    public List<Unit> GetTotalUnitList() => add(totalPlayerUnits, totalEnemyUnits);
    public List<Unit> GetEnemyUnitList() => totalEnemyUnits;
    public List<Unit> GetUnitList() => totalPlayerUnits;
    public void RemoveUnitFromList(Unit unit, bool isEnemy) {
        if(isEnemy) {
            totalEnemyUnits.Remove(unit);
            OnEnemyDestroy?.Invoke();
        } else {
            totalPlayerUnits.Remove(unit);
        }
    }
    public void AddUnitToList(Unit unit, bool isEnemy) {
        if(isEnemy) {
            totalEnemyUnits.Add(unit);
        } else {
            totalPlayerUnits.Add(unit);
        }
    } 
    private List<Unit> add(List<Unit> a, List<Unit> b) {
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
