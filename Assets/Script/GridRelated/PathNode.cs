using System.IO;
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

    private bool isBlock;

    public PathNode(GridPosition gridPosition) {
        this.gridPosition = gridPosition;
        gValue = int.MaxValue;
        parentNode = this;
        fValue = int.MaxValue;
        isBlock = false;
    }
    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public void Refresh(){
        gValue = int.MaxValue;
        parentNode = this;
        hValue = -1;
        fValue = int.MaxValue;
    }

    public int GetHValue() => hValue;
    public int GetGValue() => gValue;
    public int GetFValue() => fValue;

    public void SetHValue(int newH){ hValue = newH;}
    public void SetGValue(int newG){ hValue = newG;}

    public void UpdateFValue() {
        fValue = hValue + gValue;
    }
    public void SetParentNode(PathNode parentNode) {
        this.parentNode = parentNode;
    }
    public PathNode GetParentNode() => parentNode;
    public GridPosition GetGridPosition() {
        return gridPosition;
    }

    public bool IsBlock() => isBlock;

    public void SetBlock(bool status) {
        isBlock = status;
    }
}
