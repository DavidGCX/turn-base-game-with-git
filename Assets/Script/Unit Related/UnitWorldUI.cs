using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UnitWorldUI : MonoBehaviour
{

    [SerializeField] private Transform ActionPointContainer;

    [SerializeField] private Transform ActionPointReadyPrefab;
    [SerializeField] private Transform ActionPointSelectedPrefab;

    [SerializeField] private Transform ActionPointUsedPrefab;

    [SerializeField] private TMP_Text ActionPointCount;
    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }
    // Start is called before the first frame update

    public void HandleActionPoint() {
        if (UnitActionsystem.Instance.GetSelectedUnit() != unit || 
        UnitActionsystem.Instance.GetSelectedAction() == null){
            UpdateActionPoint(0);
        } else{
            UpdateActionPoint(UnitActionsystem.Instance.GetSelectedAction().GetActionSpent());
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
}
