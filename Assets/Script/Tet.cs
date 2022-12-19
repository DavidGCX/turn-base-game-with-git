using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tet : MonoBehaviour
{
    [SerializeField] private Unit unit;
    
    // Start is called before the first frame update
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) {
            GridsystemVisual.instance.HideAllGridPosition();
            GridsystemVisual.instance.ShowGridPositonList(unit.GetMoveAction().GetValidGridPositionList());
            //MoveAction.Instance.GetValidGridPositionList();
        }
        
    }
}
