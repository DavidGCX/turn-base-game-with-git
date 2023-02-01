using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/* Base building:
    - scroll view for units
    - buttons for units

*/
public class BaseBuildingUI : MonoBehaviour
{   
    /*
    public GameObject prefab; // This is our prefab obj that will be exposed in the inspector
    public int numberToCreate; // exposed in inspector

    void Populate() {
        GameObject newObj;

        for (int i=0; i<numberToCreate; i++) {
            // Create new instances of our prefab
            newObj = (GameObject)Instantiate(prefab, transform);
            // Randomize color of image
            newObj.GetComponent().color = Random.ColorHSV();
        }
    }*/
    // [SerializeField] private TMP_Text text;
    // [SerializeField] private Button button;

    // [SerializeField] private GameObject selectedVisual;

    // [SerializeField] private Transform actionPointButtonVisual;

    // [SerializeField] private Transform actionPointButtonVisualContainer;
    // private BaseAction baseAction;
    
    // public void SetUpBaseAction (BaseAction baseAction) {
    //     this.baseAction = baseAction;
    //     text.text = baseAction.GetActionName();
    //     for (int i = 0; i < baseAction.GetActionSpent(); i++)
    //     {
    //         Instantiate(actionPointButtonVisual, actionPointButtonVisualContainer);
    //     }
    //     button.onClick.AddListener(() => {
    //         if (UnitActionSystem.Instance.GetSelectedUnit().CanSpendActionPoint(baseAction)) {
    //             UnitActionSystem.Instance.SetSelectedAction(baseAction);
    //         } else {
    //             UnitActionSystem.Instance.SendNotification("This action needs more action point than this unit have");
    //         }
    //     });
    // }

    // public void UpdateSelectedVisual(){
    //     //Debug.Log("This one is called");
    //     BaseAction selectedOne = UnitActionSystem.Instance.GetSelectedAction();
    //     //Debug.Log(selectedOne == baseAction);
    //     selectedVisual.SetActive(selectedOne == baseAction);
    // }


}
