using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TurnSystemUI : MonoBehaviour
{
    
    [SerializeField] private Button endTurnButton;
    
    [SerializeField] private TMP_Text turnNumber;

    [SerializeField] private GameObject EnemyTurnVisual;


    private void Start()
    {
        //Debug.Log("TURN: " + TurnSystem.instance.GetTurnNumber());
        endTurnButton.onClick.AddListener(TurnSystem.instance.NextTurn);
        TurnSystem.instance.OnTurnChange += UpdateWhenOnTurnChange;
        TurnSystem.instance.NextTurn();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButton();
    }
    private void Update()
    {
        
    }

    private void UpdateWhenOnTurnChange() {
        turnNumber.text = "TURN: " + TurnSystem.instance.GetTurnNumber();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButton();
    }
    private void UpdateEnemyTurnVisual() {
        EnemyTurnVisual.SetActive(!TurnSystem.instance.IsPlayerTurn());
    }    

    private void UpdateEndTurnButton() {
        endTurnButton.gameObject.SetActive(TurnSystem.instance.IsPlayerTurn());
    }
}
