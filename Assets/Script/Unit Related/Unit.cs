using System.Reflection;
using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    private SpinAction spinAction;
    private MoveAction moveAction;
    private const int MINIMUMATTACK = 10;
    private const int MINIMUMDENFENSE = 10;
    [SerializeField] private int currentActionPoint = 5;

    [SerializeField] private int maximumActionPoint = 5;
    [SerializeField] private int attack = 25;
    [SerializeField] private int defense = 0;
    [SerializeField] private int baseAttack = 40;
    [SerializeField] private int armor = 30;
    [SerializeField] private int health = 100;
    [SerializeField] private Transform ActionPointContainer;

    [SerializeField] private Transform ActionPointReadyPrefab;
    [SerializeField] private Transform ActionPointSelectedPrefab;

    [SerializeField] private Transform ActionPointUsedPrefab;

    [SerializeField] private TMP_Text ActionPointCount;

    [SerializeField] private bool isEnemy = false;

    private bool isDead = false;

    private BaseAction[] baseActions;
    private GridPosition lastGridPosition;

    [SerializeField]private List<CurrentStatus> statusList;

    public enum CurrentStatus {
      CanNotShoot,
    }

    public enum Stats {
        attack,
        defense,
        armor,
        health,
    }

   
    private void Awake()
    {
        statusList = new List<CurrentStatus>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActions = GetComponents<BaseAction>();
        isDead = false;
    }
    
    
    private void Start()
    {
        GridPosition gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        lastGridPosition = gridPosition;
        LevelGrid.instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.instance.OnTurnChange += NewTurn;

    }
    
    void Update()
    {
        GridPosition gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        if (gridPosition.x < 0) {
            gridPosition.x = 0;
            // Debug.Log($"Out of Boundary, current pos is {LevelGrid.instance.GetGridPosition(transform.position)}");
        } 
        if (gridPosition.z < 0) {
            //Debug.Log($"Out of Boundary, current pos is {LevelGrid.instance.GetGridPosition(transform.position)}");
            gridPosition.z = 0;
        }
        if (lastGridPosition != gridPosition) {
            LevelGrid.instance.UnitMoveGridPosition(this, lastGridPosition, gridPosition);
            lastGridPosition = gridPosition;
        }
        HandleActionPoint();
    }



    /* public SpinAction GetSpinAction() {
        return spinAction;
    }*/
    

    public bool CanSpendActionPoint(BaseAction baseAction) {
        if (baseAction.GetActionSpent() > currentActionPoint) {
            return false;
        } else {
            return true;
        }
    }

    public bool TrySpendActionPoint(BaseAction baseAction) {
        if (CanSpendActionPoint(baseAction)) {
            currentActionPoint -= baseAction.GetActionSpent();
            return true;
        } else {
            return false;
        }
    }

    public void NewTurn() {
        if(isEnemy && !TurnSystem.instance.IsPlayerTurn() ||
        !isEnemy && TurnSystem.instance.IsPlayerTurn()) {
            currentActionPoint = maximumActionPoint;
        }
    }

    public void HandleActionPoint() {
        if (UnitActionsystem.Instance.GetSelectedUnit() != this || 
        UnitActionsystem.Instance.GetSelectedAction() == null){
            UpdateActionPoint(0);
        } else{
            UpdateActionPoint(UnitActionsystem.Instance.GetSelectedAction().GetActionSpent());
        }
        ActionPointCount.text = $"{currentActionPoint}/{maximumActionPoint}";
    }

    public void UpdateActionPoint(int selectedAmount) {
        ClearPanel();
        int max = maximumActionPoint;
        int current = currentActionPoint - selectedAmount;
        int used = max - current - selectedAmount;
        //Debug.Log(used);
        if(current >= 0) {
            for (int i = 0; i < selectedAmount; i++)
            {
                Instantiate(ActionPointSelectedPrefab,ActionPointContainer);
            }
            for (int j = 0; j < current; j++)
            {
                Instantiate(ActionPointReadyPrefab,ActionPointContainer);
            }
            for (int k = 0; k < used; k++)
            {
                Instantiate(ActionPointUsedPrefab,ActionPointContainer);
            }
        } else {
            current += selectedAmount;
            for (int j = 0; j < current; j++)
            {
                Instantiate(ActionPointReadyPrefab,ActionPointContainer);
            }
            for (int i = 0; i < max - current; i++)
            {
                Instantiate(ActionPointUsedPrefab,ActionPointContainer);
            }
        }
    }

    public void ClearPanel() {
        //Debug.Log("This one is called");
        foreach (Transform item in ActionPointContainer)
        {
            Destroy(item.gameObject);
        }
    }

    public void Damage(int baseDamage, int apDamage, int totalAttack, float damageRandomRate) {
        int currentAttack;
        int currentDefense;
        currentDefense = defense < 10 ? MINIMUMDENFENSE : defense;
        currentAttack = totalAttack - currentDefense < MINIMUMATTACK? MINIMUMATTACK:totalAttack - currentDefense;
        int randomNumber = UnityEngine.Random.Range(1, 101);
        if (currentAttack < randomNumber) {
            Debug.Log($"The attack was defensed \n with attack: {currentAttack} randomNumber: {randomNumber}");
            return;
        }

        Debug.Log($"Attack Hit the Base Damage is {baseDamage}, the AP damage is {apDamage}");
        float randomDamageRate = UnityEngine.Random.Range(- damageRandomRate,  damageRandomRate)/ 100f;
        int totalDamage =  Mathf.RoundToInt((apDamage + CalculateBaseDamageAfterArmor(baseDamage)) * (1 + randomDamageRate));
        Debug.Log($"Total Damage is {totalDamage}");
        if(health - totalDamage < 0){isDead = true;}
        health = health - totalDamage < 0? 0 : health - totalDamage;
        Debug.Log($"Current Health is {health}");
    }

    private int CalculateBaseDamageAfterArmor(int baseDamage) {
        float reduceRate = ((float)armor) / 200f;
        
        float resultDamage = ((float)baseDamage) * (1 - reduceRate);
        return Mathf.RoundToInt(resultDamage);
    }

    public void ClearStatus() {
        statusList.Clear();
    }

    public void AddStatus(CurrentStatus unitStatus) {
        statusList?.Add(unitStatus);
    }

    public void RemoveStatus(CurrentStatus unitStatus) {
        statusList?.Remove(unitStatus);
    }

    public void SetStats(int amount, Stats stat){
        switch (stat)
        {
            case Stats.armor:
                armor = amount;
                break;
            case Stats.attack:
                attack = amount;
                break;
            case Stats.defense:
                defense = amount;
                break;
            case Stats.health:
                health = amount;
                break;
        }
    }

    public int GetStats(Stats stat){
        switch (stat)
        {
            case Stats.armor:
                return armor;
            case Stats.attack:
                return attack;
            case Stats.defense:
                return defense;
            case Stats.health:
                return health;
            default:
                Debug.Log("Get Wrong Stats");
                return -1;
        }
    }
    public bool CheckStatus(CurrentStatus unitStatus) => statusList.Contains(unitStatus);

    public GridPosition GetGridPosition() => lastGridPosition;

     public Vector3 GetWorldPosition() => LevelGrid.instance.GetWorldPosition(lastGridPosition);


    public BaseAction[] GetBaseActions() => baseActions;
    public int GetCurrentActionPoint() => currentActionPoint;

    public int GetMaxActionPoint() => maximumActionPoint;
    public MoveAction GetMoveAction() => moveAction;

    public bool GetUnitType() => isEnemy;
    public void SetUnitType(bool type) {isEnemy = type;}

    public int GetUnitAttackTotal() => baseAttack + attack;
    public int GetUnitAttackBase() => baseAttack;
}

