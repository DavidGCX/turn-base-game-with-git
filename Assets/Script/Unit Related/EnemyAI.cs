using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        TurnSystem.instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnSystem.instance.IsPlayerTurn()) {
            return;
        }
        timer -= Time.deltaTime;
        if(timer <= 0f) {
            TurnSystem.instance.NextTurn();
        }
    }

    private void TurnSystem_OnTurnChange() {
        timer = 2f;
    } 
}
