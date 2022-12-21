using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridsystemVisual : MonoBehaviour
{
    public static GridsystemVisual instance {get; private set;}
    private GridSystemVisualSingle[,] visualSingles;
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    // Start is called before the first frame update
    
    private void Awake() {
        instance = this;
    }
    private void Start()
    {
       // Debug.Log("This one is called");
        visualSingles = new GridSystemVisualSingle[LevelGrid.instance.GetWidth(), LevelGrid.instance.GetWidth()];
        for (int x = 0; x < LevelGrid.instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.instance.GetWorldPosition(gridPosition), Quaternion.identity);
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

    private void UpdateGridVisual() {
        HideAllGridPosition();
        Unit unit = UnitActionsystem.Instance.GetSelectedUnit();
        if(unit == null) {return;}
        if(UnitActionsystem.Instance.GetSelectedAction() == null) {return;}
        ShowGridPositonList(UnitActionsystem.Instance.GetSelectedAction().GetValidGridPositionList());
    }

    private void Update() {
        UpdateGridVisual();
    }
    public void UnitActionsystem_OnSelectedEvent_UpdateActionPoint() {
    
    }

}
