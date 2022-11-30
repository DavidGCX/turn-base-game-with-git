using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    private const float stopDistance = 0.1f;
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if(Vector3.Distance(transform.position, targetPosition) > stopDistance) {
            transform.LookAt(targetPosition);
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }
        if (Input.GetMouseButtonDown(0)) {
            Move(MouseWorld.GetMousePosition());
        }
    }
    private void Move(Vector3 targetPosition) {
        this.targetPosition  = targetPosition;
    }
}
