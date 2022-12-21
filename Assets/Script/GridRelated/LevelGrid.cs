using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid instance {get; private set;}
    [SerializeField] private Transform debugPrefab;

    [SerializeField] private int cellSize = 2; 
    [SerializeField] private int width = 10; 
    [SerializeField] private int height = 10; 
    public event Action newGridSystemGenerated;
    [SerializeField] private Transform debugContainer;
    [SerializeField] private GridsystemVisual gridSystemVisual;
    private GridSystem gridSystem;
    private void Awake() {
        if(instance != null) {
            Debug.Log("You should delete the old LevelGrid");
            return;
        }
        instance = this;
        gridSystem = new GridSystem(width, height, cellSize, transform.position, new Vector3(0, transform.position.y, 0), debugContainer);
        gridSystem.CreateDebugObject(debugPrefab);
    }

    public void CreateANewGridSystem(Transform transform) {
        foreach (GridDebugObject item in debugContainer.GetComponentsInChildren<GridDebugObject>())
        {
            Destroy(item.gameObject);
        }
        gridSystem = new GridSystem(width, height, cellSize, transform.position, new Vector3(0, transform.position.y, 0), debugContainer);
        gridSystemVisual.GridSystemVisualGenerate();
        gridSystem.CreateDebugObject(debugPrefab);
    }
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit) {
        GridObject CurrentGridObject = gridSystem.GetGridObject(gridPosition);
        CurrentGridObject.AddUnit(unit);
    }
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition) {
        GridObject CurrentGridObject = gridSystem.GetGridObject(gridPosition);
        return CurrentGridObject.GetUnitList();
    }
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit) {
        GridObject CurrentGridObject = gridSystem.GetGridObject(gridPosition);
        CurrentGridObject.RemoveUnit(unit );
    }

    public void UnitMoveGridPosition(Unit unit, GridPosition from, GridPosition to){
        RemoveUnitAtGridPosition(from, unit);
        AddUnitAtGridPosition(to, unit);
    }

    public GridPosition GetGridPosition(Vector3 worldpos) => gridSystem.GetGridPosition(worldpos);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsAValidGridPosition(GridPosition gridPosition) => gridSystem.IsAValidGridPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition) => GetUnitListAtGridPosition(gridPosition).Count != 0;

    public int GetWidth() => gridSystem.Getwidth();
    public int GetHeight() => gridSystem.Getheight();

}
