using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private Unit unit;
    private GridPosition gridPosition;
    private GridSystem gridSystem;

    public GridObject(GridSystem a, GridPosition b) {
        gridPosition = b;
        gridSystem = a;
    }

    public void SetUnit(Unit unit) {
        this.unit = unit;
    }

    public Unit GetUnit() {
        return unit;
    }

    public override string ToString()
    {
        return gridPosition.ToString() + "\n"+unit;
    }
}
