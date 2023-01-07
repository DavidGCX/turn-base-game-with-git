using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private float timer;
    private State state;
    // Start is called before the first frame update
    private bool insideCoroutine;

    private enum State {
        WaitingForNextTurn,
        TakingTurn,
        Busy
    }
    void Awake()
    {
        state = State.WaitingForNextTurn;
        insideCoroutine = false;
    }

    void Start()
    {
        TurnSystem.instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    private void TurnSystem_OnTurnChange()
    {
        if (!TurnSystem.instance.IsPlayerTurn()) {
            state = State.TakingTurn;
        }
    }

    // Update is called once per frame
    void Update()
    {
       switch (state) {
        case State.WaitingForNextTurn:
            insideCoroutine = false;
            break;

        case State.TakingTurn:
            if(!insideCoroutine) {
                StartCoroutine("TakeAction");
            }
            break;
        case State.Busy:
            break;
       }
    }

    private IEnumerator TakeAction() {
        insideCoroutine = true;
        yield return new WaitForSeconds(0.5f);
        if (TryEnemyAiAction(OnEnemyActionComplete)) {
            state = State.Busy;
        } else {
            TurnSystem.instance.NextTurn();
            state = State.WaitingForNextTurn;
        }
        
    }

    private bool TryEnemyAiAction(Action OnEnemyActionComplete) {
        foreach(Unit enemyUnit in UnitActionSystem.Instance.GetEnemyUnitList()) {
            if (TryTakeAction(enemyUnit, OnEnemyActionComplete)) {
                return true;
            }
        }
        return false;
    }

    private bool TryTakeAction(Unit enemyUnit, Action OnEnemyActionComplete){
            GridPosition convertedPosition = enemyUnit.GetGridPosition();
            BaseAction selectedAction = enemyUnit.GetSpinAction();
            //enemyUnit.HandleActionPointForEnemy(selectedAction);
            if(!selectedAction.IsValidMoveGridPosition(convertedPosition)) {
                return false;
            }
            if(!enemyUnit.TrySpendActionPoint(selectedAction)){
                return false;
            }
            if(!selectedAction.HandleUnitState()) {
                return false;
            }
            
            selectedAction.TakeAction(convertedPosition, OnEnemyActionComplete);
            enemyUnit.HandleActionPointForEnemy(null);
            return true;
    }


    private void OnEnemyActionComplete() {
        insideCoroutine = false;
        state = State.TakingTurn;
    }
}
