using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionsystem : MonoBehaviour
{
    [SerializeField] private LayerMask UnitySelectLayerMask;
    [SerializeField] Unit selectedUnit;
    private void Awake() {
    }
    
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (TryHandleUnitSelection()) {
            } else {
                selectedUnit.Move(MouseWorld.GetMousePosition());
            } 
        } 
    }

    private bool TryHandleUnitSelection() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, UnitySelectLayerMask)) {
            if(rayCastHit.transform.TryGetComponent<Unit>(out Unit unit)) {
                selectedUnit = unit;
                return true;
            }
        }
        return false;
    }
}
