using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform ButtonUI;
    [SerializeField] private Transform ButtonUIContainer;
    // Start is called before the first frame update
    void Start()
    {
        UnitActionsystem.Instance.SelectEvent += UnitActionSystem_OnSelectionChange;
        CreateUnitActionButton();
    }

    private void CreateUnitActionButton() {
        foreach (Transform item in ButtonUIContainer)
        {
            Destroy(item.gameObject);
        }
        Unit SelectedUnit = UnitActionsystem.Instance.GetSelectedUnit();
        foreach (var baseAction in SelectedUnit.GetBaseActions()) {
            Transform actionButtonTransform = Instantiate(ButtonUI, ButtonUIContainer);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetUpBaseAction(baseAction);
        }
    }

    private void UnitActionSystem_OnSelectionChange() {
        CreateUnitActionButton();
    }

}
