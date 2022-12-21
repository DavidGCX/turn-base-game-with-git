using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridSystem
{
    private int width;
    private int height;
    private int cellSize;

    private Vector3 heightAdjustMent;
    private Vector3 StandarizeVectior = new Vector3(0 ,0 ,0);

    private GridObject[,] gridObjectArray;

    
    public GridSystem(int width, int height, int cellSize, Vector3 stVector, Vector3 hgVector) {
        this.cellSize = cellSize;
        this.width = width;
        this.height = height;
        this.StandarizeVectior = GetWorldPosition(GetGridPosition(stVector));
        this.heightAdjustMent = hgVector;
        gridObjectArray = new GridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x,z] = new GridObject(this, gridPosition);
                Debug.DrawLine(GetWorldPosition(gridPosition), GetWorldPosition(gridPosition) + Vector3.right *0.3f, Color.red, 1000);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition) {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize + StandarizeVectior + heightAdjustMent;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) {
        return new GridPosition (
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        ) - new GridPosition (
            Mathf.RoundToInt(StandarizeVectior.x / cellSize),
            Mathf.RoundToInt(StandarizeVectior.z / cellSize)
        );
    }

    public void CreateDebugObject(Transform debugPrefab) {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugObject= GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugObject.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridDebugObject(gridObjectArray[x,z]);
            }
            
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition){
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsAValidGridPosition(GridPosition gridPosition) =>
        !(gridPosition.x < 0 || gridPosition.z < 0) && 
        gridPosition.x < width &&
        gridPosition.z < height;

    public int Getwidth() => width;
    public int Getheight() => height;
}
