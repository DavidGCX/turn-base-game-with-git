using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private State state;
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
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    private void TurnSystem_OnTurnChange()
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) {
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
            TurnSystem.Instance.NextTurn();
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

            List<EnemyAIAction> enemyAIActions = new List<EnemyAIAction>();
            foreach (BaseAction baseAction in enemyUnit.GetBaseActions())
            {
                if(enemyUnit.CanSpendActionPoint(baseAction)) {
                    EnemyAIAction partialBestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    if (partialBestEnemyAIAction != null) {
                        enemyAIActions.Add(baseAction.GetBestEnemyAIAction());
                    }
                }
            }
            if (enemyAIActions.Count == 0) {
                return false;
            }
            
            enemyAIActions.Sort((EnemyAIAction actionOne, EnemyAIAction actionTwo) => actionTwo.actionValue - actionOne.actionValue);
            EnemyAIAction bestEnemyAiAction = enemyAIActions[0];
            if(!bestEnemyAiAction.baseAction.HandleUnitState()) {
                return false;
            }
            if(!enemyUnit.TrySpendActionPoint(bestEnemyAiAction.baseAction)){
                return false;
            }
            bestEnemyAiAction.baseAction.TakeAction(bestEnemyAiAction.gridPosition, OnEnemyActionComplete);
            CameraController.Instance.FocusOnWorldPositon(enemyUnit.GetWorldPosition());
            enemyUnit.HandleActionPointForEnemy(null);
            return true;
    }


    private void OnEnemyActionComplete() {
        insideCoroutine = false;
        state = State.TakingTurn;
    }
}
