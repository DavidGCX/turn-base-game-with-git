using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform ButtonUI;
    [SerializeField] private Transform ButtonUIContainer;

    private List<ActionButtonUI> actionButtonUIs;

    private void Awake() {
        actionButtonUIs = new List<ActionButtonUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform item in ButtonUIContainer)
        {
            Destroy(item.gameObject);
        }
        UnitActionsystem.Instance.SelectEvent += UnitActionSystem_OnSelectionChange;
        UnitActionsystem.Instance.OnSelectedActionChange += UpdateSelectedVisual;
    }

    private void CreateUnitActionButton() {
        foreach (Transform item in ButtonUIContainer)
        {
            Destroy(item.gameObject);
        }
        actionButtonUIs.Clear();
        Unit SelectedUnit = UnitActionsystem.Instance.GetSelectedUnit();
        foreach (var baseAction in SelectedUnit.GetBaseActions()) {
            Transform actionButtonTransform = Instantiate(ButtonUI, ButtonUIContainer);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetUpBaseAction(baseAction);

            actionButtonUIs.Add(actionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectionChange() {
        
        CreateUnitActionButton();
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual() {
        //Debug.Log("Called");
        foreach (var buttonUI in actionButtonUIs)
        {
            buttonUI.UpdateSelectedVisual();
        }
    }
}
