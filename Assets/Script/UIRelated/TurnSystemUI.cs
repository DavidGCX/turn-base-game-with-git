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
        endTurnButton.onClick.AddListener(TurnSystem.Instance.NextTurn);
        TurnSystem.Instance.OnTurnChange += UpdateWhenOnTurnChange;
        UnitActionSystem.Instance.OnBusyChange +=  UpdateEndTurnButton;
        TurnSystem.Instance.NextTurn();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButton();
    }
    private void Update()
    {
        
    }

    private void UpdateWhenOnTurnChange() {
        turnNumber.text = "TURN: " + TurnSystem.Instance.GetTurnNumber();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButton();
    }
    private void UpdateEnemyTurnVisual() {
        EnemyTurnVisual.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }    

    private void UpdateEndTurnButton() {
        endTurnButton.gameObject.SetActive(!UnitActionSystem.Instance.GetBusyStatus());
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
