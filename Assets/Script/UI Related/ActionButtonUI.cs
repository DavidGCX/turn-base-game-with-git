using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button button;
    private void Start() {
    }

    public void SetUpBaseAction (BaseAction baseAction) {
        text.text = baseAction.GetActionName();
        button.onClick.AddListener(() => {
            UnitActionsystem.Instance.SetSelectedAction(baseAction);
        });
    } 
}
