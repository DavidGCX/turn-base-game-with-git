using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraMoveSpeed = 5f;
     [SerializeField] private float cameraRotateSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputMoveDir = new Vector3(0, 0, 0);  
        Vector3 rotationDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = 1;
        } 
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1;
        } 
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1;
        } 
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = 1;
        } 
        if (Input.GetKey(KeyCode.Q))
        {
           rotationDir.y = 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationDir.y = -1f;
        }
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * cameraMoveSpeed * Time.deltaTime;
        transform.eulerAngles += rotationDir * cameraRotateSpeed * Time.deltaTime;
    }
}
