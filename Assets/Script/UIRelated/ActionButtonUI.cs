using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button button;

    [SerializeField] private GameObject selectedVisual;

    [SerializeField] private Transform actionPointButtonVisual;

    [SerializeField] private Transform actionPointButtonVisualContainer;
    private BaseAction baseAction;
    
    public void SetUpBaseAction (BaseAction baseAction) {
        this.baseAction = baseAction;
        text.text = baseAction.GetActionName();
        for (int i = 0; i < baseAction.GetActionSpent(); i++)
        {
            Instantiate(actionPointButtonVisual, actionPointButtonVisualContainer);
        }
        button.onClick.AddListener(() => {
            if (UnitActionSystem.Instance.GetSelectedUnit().CanSpendActionPoint(baseAction)) {
                UnitActionSystem.Instance.SetSelectedAction(baseAction);
            } else {
                UnitActionSystem.Instance.SendNotification("This action needs more action point than this unit have");
            }
        });
    }

    public void UpdateSelectedVisual(){
        //Debug.Log("This one is called");
        BaseAction selectedOne = UnitActionSystem.Instance.GetSelectedAction();
        //Debug.Log(selectedOne == baseAction);
        selectedVisual.SetActive(selectedOne == baseAction);
    }


}
