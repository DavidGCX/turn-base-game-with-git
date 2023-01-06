using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private AttackAction attackFrom;
    private Vector3 moveDriection;
    private Vector3 targetPosition;
    [SerializeField] GameObject bulletHitEffectPrefab;
    
    private void Awake() {
        attackFrom = (AttackAction) UnitActionSystem.Instance.GetSelectedAction();
    }

    public void SetUp(AttackAction attackAction, Vector3 moveDirection, Vector3 targetPosition) {
        this.attackFrom = attackAction;
        this.moveDriection = moveDirection;
        this.targetPosition = targetPosition;
        GetComponent<Rigidbody>().AddForce(moveDriection * 10000);
    }

    void Update()
    {  
       
        //float MoveSpeed = 60f;
        //transform.position += MoveSpeed * moveDriection * Time.deltaTime;
        transform.LookAt(targetPosition);
        
    }

    

    void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<Unit> (out Unit unit)) {
                Debug.Log(other);
        }
         
    }
    void OnCollisionEnter(Collision other)
    {
        //Instantiate(bulletHitEffectPrefab, transform.position, Quaternion.identity);
        if(other.gameObject.tag == "Unit") {
            Debug.Log(other);
            if(other.gameObject.TryGetComponent<Unit> (out Unit unit)) {
                if (attackFrom.CauseDamage(unit)) {
                     Instantiate(bulletHitEffectPrefab, transform.position, Quaternion.identity);
                }
               
            }
            
        }
        //Debug.Log(other);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    { 
        
        /*
        
        */
        //Destroy(gameObject);
        
    }

}
