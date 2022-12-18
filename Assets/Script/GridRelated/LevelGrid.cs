using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid instance {get; private set;}
    [SerializeField] private Transform debugPrefab;
    private GridSystem gridSystem;
    private void Awake() {
        instance = this;
        gridSystem = new GridSystem(10, 10, 2);
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

}
