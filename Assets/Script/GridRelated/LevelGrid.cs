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
    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit) {
        GridObject CurrentGridObject = gridSystem.GetGridObject(gridPosition);
        CurrentGridObject.SetUnit(unit);
    }
    public Unit GetUnitAtGridPosition(GridPosition gridPosition) {
        GridObject CurrentGridObject = gridSystem.GetGridObject(gridPosition);
        return CurrentGridObject.GetUnit();
    }
    public void ClearUnitAtGridPosition(GridPosition gridPosition) {
        GridObject CurrentGridObject = gridSystem.GetGridObject(gridPosition);
        CurrentGridObject.SetUnit(null);
    }

    public void UnitMoveGridPosition(Unit unit, GridPosition from, GridPosition to){
        ClearUnitAtGridPosition(from);
        SetUnitAtGridPosition(to, unit);
    }

    public GridPosition GetGridPosition(Vector3 worldpos) => gridSystem.GetGridPosition(worldpos);


}
