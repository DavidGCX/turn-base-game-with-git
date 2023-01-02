using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    AttackAction AttackFrom;
    
    private void Awake() {
        AttackFrom = (AttackAction) UnitActionSystem.Instance.GetSelectedAction();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Unit") {
            Debug.Log(other);
            if(other.TryGetComponent<Unit> (out Unit unit)) {
                AttackFrom.CauseDamage(unit);
            }
            
        }
        
    }

}
