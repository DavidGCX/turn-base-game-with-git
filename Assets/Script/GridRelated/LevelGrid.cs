using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    
    [Header("GridSystem基础参数")]
    [Tooltip("每个格子的大小")]
    [SerializeField] private int cellSize = 2;
    [Tooltip("宽度")]
    [SerializeField] private int width = 10;
    [Tooltip("高度")]
    [SerializeField] private int height = 10;
    //public event Action newGridSystemGenerated;
    [Header("必要引用")]
    [SerializeField] private Transform debugPrefab;
    [SerializeField] private Transform debugContainer;
    [SerializeField] private GridSystemVisual gridSystemVisual;

    public event Action OnAnyUnitChangePosition;
    private GridSystem<GridObject> gridSystem;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("You should delete the old LevelGrid");
            return;
        }
        Instance = this;
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, transform.position, new Vector3(0, transform.position.y, 0),
            debugContainer, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        gridSystem.CreateDebugObject(debugPrefab);
    }

    public void CreateANewGridSystem(Transform transform)
    {
        foreach (GridDebugObject item in debugContainer.GetComponentsInChildren<GridDebugObject>())
        {
            Destroy(item.gameObject);
        }
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, transform.position, new Vector3(0, transform.position.y, 0),
        debugContainer, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        gridSystemVisual.GridSystemVisualGenerate();
        gridSystem.CreateDebugObject(debugPrefab);
    }

    public GridSystem<PathNode> CreateANewGridSystemPathNode()
    {
        return new GridSystem<PathNode>(width, height, cellSize, transform.position, new Vector3(0, transform.position.y, 0),
        debugContainer, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
    }


    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject CurrentGridObject = gridSystem.GetGridObject(gridPosition);
        CurrentGridObject.AddUnit(unit);
    }
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject CurrentGridObject = gridSystem.GetGridObject(gridPosition);
        return CurrentGridObject.GetUnitList();
    }
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject CurrentGridObject = gridSystem.GetGridObject(gridPosition);
        CurrentGridObject.RemoveUnit(unit);
    }

    public void UnitMoveGridPosition(Unit unit, GridPosition from, GridPosition to)
    {
        RemoveUnitAtGridPosition(from, unit);
        AddUnitAtGridPosition(to, unit);
        OnAnyUnitChangePosition?.Invoke();
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsAValidGridPosition(GridPosition gridPosition) => gridSystem.IsAValidGridPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition) => GetUnitListAtGridPosition(gridPosition).Count != 0;

    public Unit GetUnitAtGridPosition(GridPosition gridPosition) => GetUnitListAtGridPosition(gridPosition)[0];

    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();

    public int GetGridDistance(Vector3 unit, Vector3 target) => gridSystem.GetGridDistance(unit, target);

    public int GetGridDistance(Unit unit, Unit target) => gridSystem.GetGridDistance(unit, target);

    public int GetCellSize() => cellSize;
}
