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
        yield return new WaitForSeconds(2f);
        TurnSystem.instance.NextTurn();
        state = State.WaitingForNextTurn;
        
    }
}
