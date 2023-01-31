using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUnit : Unit
{
    protected override void Awake()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        lastGridPosition = gridPosition;
        baseActions = GetComponents<BaseAction>();
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }
    protected override void Start() {

    }
    public override bool IsBuilding() => true;
    public override bool GetUnitType() => false;
}
