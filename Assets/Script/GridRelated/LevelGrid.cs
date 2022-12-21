using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid instance {get; private set;}
    [SerializeField] private Transform debugPrefab;
    private Vector3 StandarlizeVector;

    private Vector3 worldPosition;

    private int cellSize = 2; 
    private GridSystem gridSystem;
    private void Awake() {
        worldPosition = transform.position;
        instance = this;
        gridSystem = new GridSystem(10, 10, cellSize);
        gridSystem.CreateDebugObject(debugPrefab);
        
        GridPosition Level  = new GridPosition (
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        );
        StandarlizeVector = new Vector3(Level.x, 0, Level.z) * cellSize;
    }
    private void Start() {
        StandarlizeVector = GetWorldPosition(GetGridPosition(transform.position));
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

    public GridPosition GetGridPosition(Vector3 worldpos) => gridSystem.GetGridPosition(worldpos) - gridSystem.GetGridPosition(StandarlizeVector);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition) + StandarlizeVector;

    public bool IsAValidGridPosition(GridPosition gridPosition) => gridSystem.IsAValidGridPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition) => GetUnitListAtGridPosition(gridPosition).Count != 0;

    public int GetWidth() => gridSystem.Getwidth();
    public int GetHeight() => gridSystem.Getheight();

}
