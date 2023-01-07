using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    private GridSystemVisualSingle[,] visualSingles;
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private Transform visualContainer;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    // Start is called before the first frame update
    
    [Serializable]
    public struct GridVisualTypeMaterial{
        public GridVisualType gridVisualType;
        public Material material;
    }


    public enum GridVisualType {
        White_Default,
        Yellow_ForInteraction,
        Blue_ForEffectOnAllies,
        Red_ForAttackTarget,
        LightRed_ForAttackInRange
    }

    private void Awake() {
        GridSystemVisualGenerate();
        HideAllGridPosition();
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
        UnitActionSystem.Instance.OnEnemyDestroy += UnitActionSystem_OnEnemyDestroy;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        LevelGrid.instance.OnAnyUnitChangePosition += LevelGrid_OnAnyUnitChangePosition;
    }

    private void UnitActionSystem_OnEnemyDestroy()
    {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitChangePosition()
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OnSelectedActionChange()
    {
        UpdateGridVisual();
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

    public void ShowGridPositonList(List<GridPosition> gridPositions, Material material) {
        foreach(var obj in gridPositions) {
            visualSingles[obj.x, obj.z].Show(material);
        }
    }
    //Update Grid visual, if it is an AttackAction, then show the target and range differently
    private void UpdateGridVisual() {
        HideAllGridPosition();
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction baseAction = UnitActionSystem.Instance.GetSelectedAction();
        if(unit == null) {return;}
        if(baseAction == null) {return;}
        if(baseAction.IsAttackAction()) {
            AttackAction attackAction = (AttackAction) baseAction;
            ShowGridPositonList(attackAction.GetGridPositionListInRange(), GetGridMaterialFromType(attackAction.GetGridVisualTypeForRange()));
            ShowGridPositonList(attackAction.GetValidGridPositionList(),  GetGridMaterialFromType(attackAction.GetGridVisualType()));
        } else {
            ShowGridPositonList(baseAction.GetValidGridPositionList(), GetGridMaterialFromType(baseAction.GetGridVisualType()));
        }
    }
    private Material GetGridMaterialFromType(GridVisualType gridVisualType) {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType) {
                return gridVisualTypeMaterial.material;
            }
        }
        Debug.LogError("Could not find the correct material bind to this type");
        return null;
    }


}
