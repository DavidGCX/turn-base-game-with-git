using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private List<Unit> unitList;
    private GridPosition gridPosition;
    private GridSystem<GridObject> gridSystem;

    public GridObject(GridSystem<GridObject> a, GridPosition b) {
        gridPosition = b;
        gridSystem = a;
        unitList = new List<Unit>();
    }

    public void AddUnit(Unit unit) {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit) {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList() {
        return unitList;
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString += "\n" + unit;
        }
        return gridPosition.ToString() +unitString;
    }
}
