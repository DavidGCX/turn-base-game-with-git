using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    private GridPosition gridPosition;

    private int hValue = -1;
    private int gValue = -1;
    private int fValue = -1;
    private PathNode parentNode;

    public PathNode(GridPosition gridPosition) {
        this.gridPosition = gridPosition;
        gValue = int.MaxValue;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetHValue() => hValue;
    public int GetGValue() => gValue;
    public int GetFValue() => fValue;

    public void SetHValue(int newH){ hValue = newH;}
    public void SetGValue(int newG){ hValue = newG;}

    public void UpdateFValue() {
        fValue = hValue + gValue;
    }

    public GridPosition GetGridPosition() {
        return gridPosition;
    }
}
