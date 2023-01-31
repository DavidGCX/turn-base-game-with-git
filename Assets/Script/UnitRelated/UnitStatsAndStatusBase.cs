using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatsAndStatusBase : MonoBehaviour
{ 
    [Header("单位属性列表")]
    [Tooltip("单位攻击（命中率百分比），计算时为：单位攻击+基础攻击-防御=最终命中率")]
    [SerializeField] private int attack = 25;
    [Tooltip("防御（闪避率百分比），计算时为：单位攻击+基础攻击-防御=最终命中率")]
    [SerializeField] private int defense = 0;

    [Tooltip("基础攻击（基础命中率百分比），计算时为：单位攻击+基础攻击-防御=最终命中率")]
    [SerializeField] private int baseAttack = 40;

    [Tooltip("护甲，计算时：护甲/200为减伤率，减伤只减免武器基础伤害，不减免穿甲伤害")]
    [Range(0f, 200f)]
    [SerializeField] private int armor = 30;
     [Tooltip("当前生命值")]
    [SerializeField] private int currentHealth = 100;
    [Tooltip("最大生命值")]
    [SerializeField] private int maxHealth = 100;
     [Tooltip("当前行动点数")]
    [SerializeField] private int currentActionPoint = 5;
     [Tooltip("最大行动点数")]
    [SerializeField] private int maximumActionPoint = 5;

    [SerializeField] private int cost = 5;

    private const int MINIMUMATTACK = 10;
    private const int MINIMUMDENFENSE = 10;

    [Header("是否为敌方单位")]
    [SerializeField] private bool isEnemy = false;


    private bool isDead = false;
    [Header("特殊状态列表")]
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

    private void Awake() {
        statusList = new List<CurrentStatus>();
        isDead = false;
        
    }
    private void Start()
    {
        TurnSystem.Instance.OnTurnChange += NewTurn;
    }

    // Cause baseDamage + apDamage to the unit. with totalAttack hit chance and damageRandomRate to 
    // create +- damageRandomRate% random total Damage number
    public virtual bool Damage(int baseDamage, int apDamage, int totalAttack, float damageRandomRate) {
        int currentAttack;
        int currentDefense;
        currentDefense = defense < 10 ? MINIMUMDENFENSE : defense;
        currentAttack = totalAttack - currentDefense < MINIMUMATTACK? MINIMUMATTACK:totalAttack - currentDefense;
        int randomNumber = UnityEngine.Random.Range(1, 101);
        if (currentAttack < randomNumber) {
            Debug.Log($"The attack was defensed \n with attack: {currentAttack} randomNumber: {randomNumber}");
            return false;
        }

        Debug.Log($"Attack Hit the Base Damage is {baseDamage}, the AP damage is {apDamage}");
        float randomDamageRate = UnityEngine.Random.Range(- damageRandomRate,  damageRandomRate)/ 100f;
        int totalDamage =  Mathf.RoundToInt((apDamage + CalculateBaseDamageAfterArmor(baseDamage)) * (1 + randomDamageRate));
        Debug.Log($"Total Damage is {totalDamage}");
        if(currentHealth - totalDamage < 0){Dead();}
        currentHealth = currentHealth - totalDamage < 0? 0 : currentHealth - totalDamage;
        Debug.Log($"Current Health is {currentHealth}");
        return true;
    }


    //May have some features when died;
    protected virtual void Dead() {
        isDead = true;
    }

    protected virtual int CalculateBaseDamageAfterArmor(int baseDamage) {
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
                currentHealth = amount;
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
                return currentHealth;
            default:
                Debug.Log("Get Wrong Stats");
                return -1;
        }
    }
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

    public virtual void NewTurn() {
        if(isEnemy && !TurnSystem.Instance.IsPlayerTurn() ||
        !isEnemy && TurnSystem.Instance.IsPlayerTurn()) {
            currentActionPoint = maximumActionPoint;
        }
    }

    public bool CheckStatus(CurrentStatus unitStatus) => statusList.Contains(unitStatus);

    public int GetCurrentActionPoint() => currentActionPoint;

    public int GetMaxActionPoint() => maximumActionPoint;
    
    
    public int GetUnitAttackTotal() => baseAttack + attack;
    public int GetUnitAttackBase() => baseAttack;

    public bool GetUnitType() => isEnemy;
    public void SetUnitType(bool type) {isEnemy = type;}

    public float GetNormalizedHealthPercentage() => (float)currentHealth / (float)maxHealth;

    public int GetMaxHealth() => maxHealth;
    public int GetCurrentHealth() => currentHealth;

    public bool IsDead() => isDead;


    public int GetCost() => cost;
}
