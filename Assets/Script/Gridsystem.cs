using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private int cellSize;
    private GridObject[,] gridObjectArray;

    [SerializeField] private Transform debugPrefab;
    public GridSystem(int width, int height, int cellSize) {
        this.cellSize = cellSize;
        this.width = width;
        this.height = height;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x,z] = new GridObject(this, gridPosition);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right *0.3f, Color.red, 1000);
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int z) {
        return new Vector3(x, 0, z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) {
        return new GridPosition (
            Mathf.FloorToInt(worldPosition.x / cellSize),
            Mathf.FloorToInt(worldPosition.z / cellSize)
        );
    }

    public void CreateDebugObject(Transform debugPrefab) {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(x, z), Quaternion.identity);
            }
            
        }
    }
}
