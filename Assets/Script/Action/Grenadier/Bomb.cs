using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private AttackAction attackFrom;
    private Vector3 moveDriection;
    private Vector3 targetPosition;
    [SerializeField] GameObject bulletHitEffectPrefab;

    public void SetUp(AttackAction attackAction,Vector3 moveDriection, Vector3 targetPosition)
    {
        this.attackFrom = attackAction;
        this.moveDriection = moveDriection;
        this.targetPosition = targetPosition;
        Debug.Log(moveDriection);

    }
    void Update()
    {
        GetComponent<Rigidbody>().position = GetComponent<Rigidbody>().position + (moveDriection *Time.deltaTime);
        //float MoveSpeed = 60f;
        //transform.position += MoveSpeed * moveDriection * Time.deltaTime;
        transform.LookAt(targetPosition);

    }

    
    void OnCollisionEnter(Collision other)
    {
        
        ScreenShake.Instance.Shake();
        //Instantiate(bulletHitEffectPrefab, transform.position, Quaternion.identity);
        if (other.gameObject.tag == "Unit")
        {
            //Debug.Log(other);
            if (other.gameObject.TryGetComponent<Unit>(out Unit unit))
            {
                if (attackFrom.CauseDamage(unit))
                {
                    Instantiate(bulletHitEffectPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    UnitActionSystem.Instance.SendNotification("The Attack Was Defensed");
                }

            }

        }
        //Debug.Log(other);
        Destroy(gameObject);
        ScreenShake.Instance.Shake();
    }
    
    private void OnTriggerEnter(Collider other)
    {

        /*
        
        */
        //Destroy(gameObject);

    }
}
