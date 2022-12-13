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
    private GridPosition lastGridPosition;
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    private void Start()
    {
        GridPosition gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        lastGridPosition = gridPosition;
        LevelGrid.instance.SetUnitAtGridPosition(gridPosition, this);
    }
    private void Awake()
    {
        targetPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 position = new Vector3(transform.position.x, 0f, transform.position.z);
        transform.position = position;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        //Debug.Log(moveDirection);
        if(Vector3.Distance(transform.position, targetPosition) > stopDistance) {
            Quaternion quaDir = Quaternion.LookRotation(moveDirection,Vector3.up);
            //Debug.Log(quaDir); 
            float angle = Quaternion.Angle(transform.rotation, quaDir);
            if (angle > stopRotate) {
                animator.SetFloat("IdleToRun", 5, 0.1f, Time.deltaTime);
                transform.position += moveDirection * Time.deltaTime * moveSpeed*0.5f;
                transform.rotation = Quaternion.Lerp(transform.rotation,quaDir,Time.fixedDeltaTime*turnspeed);
            } else {
                transform.LookAt(targetPosition);
                transform.position += moveDirection * Time.deltaTime * moveSpeed;
                animator.SetFloat("IdleToRun", 10, 0.1f, Time.deltaTime);
            }
        } else {
            animator.SetFloat("IdleToRun", 0, 0.1f, Time.deltaTime);
        }
        GridPosition gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        if (gridPosition.x < 0) {
            gridPosition.x = 0;
             Debug.Log($"Out of Boundary, current pos is {LevelGrid.instance.GetGridPosition(transform.position)}");
        } 
        if (gridPosition.z < 0) {
            Debug.Log($"Out of Boundary, current pos is {LevelGrid.instance.GetGridPosition(transform.position)}");
            gridPosition.z = 0;
        }
        if (lastGridPosition != gridPosition) {
            LevelGrid.instance.UnitMoveGridPosition(this, lastGridPosition, gridPosition);
            lastGridPosition = gridPosition;
        }
        /* if (Input.GetMouseButtonDown(0)) {
            Move(MouseWorld.GetMousePosition());
        } */
    }
    public void Move(Vector3 targetPosition) {
        this.targetPosition  = targetPosition;
    }
}
