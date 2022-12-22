using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    
    [SerializeField] private TMP_Text turnNumber;


    private void Start()
    {
        //Debug.Log("TURN: " + TurnSystem.instance.GetTurnNumber());
        endTurnButton.onClick.AddListener(TurnSystem.instance.NextTurn);
        TurnSystem.instance.OnTurnChange += UpdateWhenOnTurnChange;
        TurnSystem.instance.NextTurn();
    }
    private void Update()
    {
        
    }

    private void UpdateWhenOnTurnChange() {
        turnNumber.text = "TURN: " + TurnSystem.instance.GetTurnNumber();
    }
}
