using System.Security.Cryptography.X509Certificates;
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

    private List<Unit> totalPlayerUnits;

    private List<Unit> totalEnemyUnits;
    [SerializeField] private GameObject BusyUI;
    [SerializeField] private Transform Notification;

    [SerializeField] private Transform unitPrefab;

    [SerializeField] Unit testUnitOne;
    [SerializeField] Unit testUnitTwo;

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
                totalPlayerUnits.Add(spawnUnit);
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
        if (selectedUnit == null) {UnitActionSystemUI.Instance.DestroyAllButton();return;}
        SetSelectedAction(selectedUnit.GetMoveAction());
        SelectEvent?.Invoke();
    }



    public void SetSelectedAction(BaseAction baseAction) {
        if(!isBusy) {
            selectedAction = baseAction;
        }
        OnSelectedActionChange.Invoke();
        
    }
    
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
        if(!selectedUnit.CanSpendActionPoint(selectedAction)){
            SetSelectedAction(null); 
        }
        BusyUI.SetActive(false);
        FinishAction?.Invoke();
    }

    public void SendNotification(string words) {
        Notification.gameObject.SetActive(true);
        Notification.gameObject.GetComponent<TMP_Text>().text = words;
    }

    public void TurnChange() {
        SetSelectedUnit(null);
    }
    public Unit GetSelectedUnit() => selectedUnit;
    public BaseAction GetSelectedAction() => selectedAction;

    public List<Unit> GetUnitList() => add(totalPlayerUnits, totalEnemyUnits);

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
