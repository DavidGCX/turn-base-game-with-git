using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridsystemVisual : MonoBehaviour
{
    private GridSystemVisualSingle[,] visualSingles;
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private Transform visualContainer;
    // Start is called before the first frame update
    
    private void Awake() {
        GridSystemVisualGenerate();
    }

    public void GridSystemVisualGenerate() {
        foreach (var item in visualContainer.GetComponentsInChildren<GridSystemVisualSingle>())
        {
            Destroy(item.gameObject);
        }
        visualSingles = new GridSystemVisualSingle[LevelGrid.instance.GetWidth(), LevelGrid.instance.GetWidth()];
        for (int x = 0; x < LevelGrid.instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.instance.GetWorldPosition(gridPosition), Quaternion.identity, visualContainer);
                //Debug.Log(gridPosition+" the gridposition");
                visualSingles[x, z] = gridVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
                
            }
        }
    }

    public void HideAllGridPosition(){
        for (int x = 0; x < LevelGrid.instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.instance.GetHeight(); z++)
            {
                visualSingles[x,z].Hide();
            }
        }
    }

    public void ShowGridPositonList(List<GridPosition> gridPositions) {
        foreach(var obj in gridPositions) {
            visualSingles[obj.x, obj.z].Show();
        }
    }
     public void ShowAttackTargetGridPositionList(List<GridPosition> gridPositions) {
        foreach(var obj in gridPositions) {
            visualSingles[obj.x, obj.z].ShowtAttackTarget();
        }
    }

    private void UpdateGridVisual() {
        HideAllGridPosition();
        Unit unit = UnitActionsystem.Instance.GetSelectedUnit();
        if(unit == null) {return;}
        if(UnitActionsystem.Instance.GetSelectedAction() == null) {return;}
        if(UnitActionsystem.Instance.GetSelectedAction().GetActionName() == "Shoot") {
            ShootAction shootAction = (ShootAction) UnitActionsystem.Instance.GetSelectedAction();
            ShowGridPositonList(shootAction.GetTargetGridPositionList());
            ShowAttackTargetGridPositionList(shootAction.GetValidGridPositionList());
        } else {
            ShowGridPositonList(UnitActionsystem.Instance.GetSelectedAction().GetValidGridPositionList());
        }
        
    }

    private void Update() {
        UpdateGridVisual();
    }

}
