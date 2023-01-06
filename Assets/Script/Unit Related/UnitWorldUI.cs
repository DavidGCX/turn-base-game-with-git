using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class UnitWorldUI : MonoBehaviour
{

    [SerializeField] private Transform ActionPointContainer;

    [SerializeField] private Transform ActionPointReadyPrefab;
    [SerializeField] private Transform ActionPointSelectedPrefab;

    [SerializeField] private Transform ActionPointUsedPrefab;

    [SerializeField] private TMP_Text ActionPointCount;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarDelay;
    [SerializeField] private TMP_Text healthNumber;
    private Unit unit;

    private int MAX_HEALTH;
    private int currentHealth;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        MAX_HEALTH = unit.GetUnitMaxHealth();
        currentHealth = unit.GetUnitCurrentHealth();
    }
    // Start is called before the first frame update

    public void HandleActionPoint() {
        if (UnitActionSystem.Instance.GetSelectedUnit() != unit || 
        UnitActionSystem.Instance.GetSelectedAction() == null){
            UpdateActionPoint(0);
        } else{
            UpdateActionPoint(UnitActionSystem.Instance.GetSelectedAction().GetActionSpent());
        }
        int currentActionPoint = unit.GetCurrentActionPoint();
        int maximumActionPoint =unit.GetMaxActionPoint();
        ActionPointCount.text = $"{currentActionPoint}/{maximumActionPoint}";
    }

    public void UpdateActionPoint(int selectedAmount) {
        ClearPanel();
        int max = unit.GetMaxActionPoint();
        int current = unit.GetCurrentActionPoint() - selectedAmount;
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
        foreach (Transform item in ActionPointContainer)
        {
            Destroy(item.gameObject);
        }
    }

    public void HandleHealth(float amount) {
        StartCoroutine(ChangeHealthBar(amount));
    }

    public IEnumerator ChangeHealthBar(float amount){
        healthBar.DOFillAmount(amount, 0.8f);
        Tween tween = healthBarDelay.DOFillAmount(amount, 1.6f);
        DOTween.To(SetHealthNumber,currentHealth, unit.GetUnitCurrentHealth(), 1.6f);
        yield return tween.WaitForCompletion();
        currentHealth = unit.GetUnitCurrentHealth();
        if(unit.IsDead()) {
            Destroy(gameObject);
        }
    }

    public void SetHealthNumber(float targetValue) {
        int value = Mathf.RoundToInt(targetValue);
        healthNumber.text = $"{value}/{MAX_HEALTH}";
    } 
}
