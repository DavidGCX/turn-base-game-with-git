using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        UnitActionSystem.Instance.SelectEventForUnit += Select;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisual();
    }

    private void Select() {
        UpdateVisual();
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.SelectEventForUnit -= Select;
    }

    private void UpdateVisual() {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit) {
            meshRenderer.enabled = true;
        } else {
            meshRenderer.enabled = false;
        }
    }
}
