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
    private BaseAction baseAction;
    
    public void SetUpBaseAction (BaseAction baseAction) {
        this.baseAction = baseAction;
        text.text = baseAction.GetActionName();
        button.onClick.AddListener(() => {
            UnitActionsystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual(){
        //Debug.Log("This one is called");
        BaseAction selectedOne = UnitActionsystem.Instance.GetSelectedAction();
        Debug.Log(selectedOne == baseAction);
        selectedVisual.SetActive(selectedOne == baseAction);
    }


}
