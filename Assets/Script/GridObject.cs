using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridPosition gridPosition;
    private GridSystem gridSystem;

    public GridObject(GridSystem a, GridPosition b) {
        gridPosition = b;
        gridSystem = a;
    }
}
