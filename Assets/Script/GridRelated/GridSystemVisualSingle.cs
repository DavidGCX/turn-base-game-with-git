using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{

    [SerializeField] private MeshRenderer Quad;

    [SerializeField] private Transform ActionPointContainer;

    [SerializeField] private Transform ActionPointReadyPrefab;
    [SerializeField] private Transform ActionPointSelectedPrefab;

    [SerializeField] private Transform ActionPointUsedPrefab;
    // Start is called before the first frame update

    public void Show() {
        Quad.enabled = true;
    }

    public void Hide() {
        Quad.enabled = false;
    } 

    public void UpdateActionPoint(Unit unit, int selectedAmount) {
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
                
                StartCoroutine("ButtonGenerateFadeIn", current);
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

    IEnumerator ButtonGenerateFadeIn(int current) {
        Instantiate(ActionPointReadyPrefab,ActionPointContainer);
        yield return new WaitForSeconds(20f);
    }
    public void ClearPanel() {
        //Debug.Log("This one is called");
        foreach (Transform item in ActionPointContainer)
        {
            Destroy(item.gameObject);
        }
    }
}
