using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform debugPrefab;
    private GridSystem grid;
    private void Awake() {
        grid = new GridSystem(10, 10, 2);
        grid.CreateDebugObject(debugPrefab);
    }
    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit) {
        GridObject CurrentGridObject = grid.GetGridObject(gridPosition);
        CurrentGridObject.SetUnit(unit);
    }
    public Unit GetUnitAtGridPosition(GridPosition gridPosition) {
        GridObject CurrentGridObject = grid.GetGridObject(gridPosition);
        return CurrentGridObject.GetUnit();
    }
    public void ClearUnitAtGridPosition(GridPosition gridPosition) {
        GridObject CurrentGridObject = grid.GetGridObject(gridPosition);
        CurrentGridObject.SetUnit(null);
    }

}
