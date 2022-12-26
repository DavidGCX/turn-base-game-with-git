using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TurnSystem : MonoBehaviour
{
    public static TurnSystem instance {get; private set;}
    private int TurnNumber = 0;
    public event Action OnTurnChange;

    private bool PlayersTurn = false;

    [SerializeField] private Animator TurnAnimator;
    // Start is called before the first frame update
    
    private void Awake()
    {
        instance = this;
    }
    public int GetTurnNumber() {
        return TurnNumber;
    }
    public void NextTurn(){
        TurnNumber++;
        PlayersTurn = !PlayersTurn;
        OnTurnChange?.Invoke();
        TurnAnimator.gameObject.SetActive(false);
        TurnAnimator.gameObject.SetActive(true);    
    }

    public bool IsPlayerTurn() => PlayersTurn;
}
