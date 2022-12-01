using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator animator; 
    private Vector3 targetPosition;
    private const float stopDistance = 0.3f;
    private const float turnspeed = 0.6f;
    private const float stopRotate = 3f;
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        //Debug.Log(moveDirection);
        if(Vector3.Distance(transform.position, targetPosition) > stopDistance) {
            Quaternion quaDir = Quaternion.LookRotation(moveDirection,Vector3.up);
            //Debug.Log(quaDir); 
            float angle = Quaternion.Angle(transform.rotation, quaDir);
            if (angle > stopRotate) {
                animator.SetFloat("IdleToRun", 0, 0.1f, Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation,quaDir,Time.fixedDeltaTime*turnspeed);
            } else {
                transform.LookAt(targetPosition);
                transform.position += moveDirection * Time.deltaTime * moveSpeed;
                animator.SetFloat("IdleToRun", 10, 0.1f, Time.deltaTime);
            }
        } else {
            animator.SetFloat("IdleToRun", 0, 0.1f, Time.deltaTime);
        }
        /* if (Input.GetMouseButtonDown(0)) {
            Move(MouseWorld.GetMousePosition());
        } */
    }
    public void Move(Vector3 targetPosition) {
        this.targetPosition  = targetPosition;
    }
}
