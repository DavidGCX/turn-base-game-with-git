using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance {get; private set;}
    private int TurnNumber = 0;

    private int flip = 0;
    public event Action OnTurnChange;

    private bool PlayersTurn = false;

    [SerializeField] private Animator TurnAnimator;
    // Start is called before the first frame update
    
    private void Awake()
    {
        Instance = this;
    }
    public int GetTurnNumber() {
        return TurnNumber;
    }
    public void NextTurn(){
        
        PlayersTurn = !PlayersTurn;
        
        if (flip == 0) {
            TurnNumber++;
            TurnAnimator.gameObject.SetActive(false);
            TurnAnimator.gameObject.SetActive(true); 
        }
        OnTurnChange?.Invoke();
        flip = (flip + 1) % 2;     
    }

    public bool IsPlayerTurn() => PlayersTurn;
}
