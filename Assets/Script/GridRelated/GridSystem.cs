using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSystem<TGridObject>
{
    private int width;
    private int height;
    private int cellSize;

    private Vector3 heightAdjustMent;
    private Vector3 standardVector = new Vector3(0 ,0 ,0);

    private TGridObject[,] gridObjectArray;
    private Transform debugContainer;

    private float CIRCLE_ADJUST_FACTOR = 0.3f;

    private Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject;
    
    public GridSystem(int width, int height, int cellSize, Vector3 stVector, Vector3 hgVector, Transform debugContainer, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject) {
        this.cellSize = cellSize;
        this.createGridObject = createGridObject;
        this.width = width;
        this.height = height;
        this.standardVector = GetWorldPosition(GetGridPosition(stVector));
        this.heightAdjustMent = hgVector;
        this.debugContainer = debugContainer;
        gridObjectArray = new TGridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x,z] = createGridObject(this, gridPosition);
                //Debug.DrawLine(GetWorldPosition(gridPosition), GetWorldPosition(gridPosition) + Vector3.right *0.3f, Color.red, 1000);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition) {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize + standardVector + heightAdjustMent;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) {
        return new GridPosition (
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        ) - new GridPosition (
            Mathf.RoundToInt(standardVector.x / cellSize),
            Mathf.RoundToInt(standardVector.z / cellSize)
        );
    }

    public void CreateDebugObject(Transform debugPrefab) {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugObject= GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity, debugContainer);
                GridDebugObject gridDebugObject = debugObject.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridDebugObject(GetGridObject(gridPosition));
            }
            
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition){
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public int GetGridDistance(Unit unit, Unit target) => GetGridDistance(unit.GetWorldPosition(), target.GetWorldPosition());
    public int GetGridDistance(Vector3 unit, Vector3 target) => Mathf.RoundToInt(Vector3.Distance(unit, target) / cellSize - CIRCLE_ADJUST_FACTOR);

    public bool IsAValidGridPosition(GridPosition gridPosition) =>
        !(gridPosition.x < 0 || gridPosition.z < 0) && 
        gridPosition.x < width &&
        gridPosition.z < height;
    public bool HasAnyWallOnGridPosition(GridPosition gridPosition)
    {
        Vector3 resultWorldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        Vector3 startPoint = resultWorldPosition;
        Vector3 direction = new Vector3(0,1,0);
        float distance = 2;
        //Debug.Log(distance);
        int ObstacleLayer = (1 << 8); // do bitwise operation to the obstacle layer
        if (Physics.Raycast(startPoint, direction, distance, ObstacleLayer))
        {
            return true;
        }
        return false;
    }
    public int GetWidth() => width;
    public int GetHeight() => height;
}
