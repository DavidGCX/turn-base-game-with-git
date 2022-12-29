using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraMoveSpeed = 5f;
    [SerializeField] private float cameraRotateSpeed = 100f;

    [SerializeField] private CinemachineVirtualCamera cameraVirtual;
    private CinemachineTransposer cinemachineTransposerCamera;
    [SerializeField] private float zoomAmount = 1f;
    
    //Camera Offset limit
    private const float CAMERA_OFFSET_MIN = 2f;
    private const float CAMERA_OFFSET_MAX = 12f;

    //Use for doing smoothing the camera movement
    private Vector3 targetOffset;
    private Vector3 targetPosition;
    private float smoothfactor = 5f;

    void Start()
    {
        cinemachineTransposerCamera = cameraVirtual.GetCinemachineComponent<CinemachineTransposer>();
        targetOffset = cinemachineTransposerCamera.m_FollowOffset;
        targetPosition = transform.position;
    }
    void Update()
    {

        HandleRotate();
        HandleMove();
        HandleZoom();
        if(Input.GetKey(KeyCode.Space)) {
            targetPosition = new Vector3(0, 0, 2);
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothfactor);     
    }

    private void HandleRotate()
    {
        Vector3 rotationDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q))
        {
           rotationDir.y = 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationDir.y = -1f;
        }
        transform.eulerAngles += rotationDir * cameraRotateSpeed * Time.deltaTime;
    }

    private void HandleMove()
    {
        Vector3 mouseScreenPosition = TranslateMouseScreenPosition();

        Vector3 inputMoveDir = new Vector3(0, 0, 0);  
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

        //Still Optional
        //if (mouseScreenPosition.magnitude >= 0.8f && !EventSystem.current.IsPointerOverGameObject()) {
            inputMoveDir.x = mouseScreenPosition.x;
            inputMoveDir.z = mouseScreenPosition.z;
        //}

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        targetPosition += moveVector * cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothfactor);
    }

    private Vector3 TranslateMouseScreenPosition() => new Vector3((Input.mousePosition.x - Screen.width * 1/2)/(Screen.width * 1/2),0,
        (Input.mousePosition.y - Screen.height * 1/2)/(Screen.height * 1/2));

    private void HandleZoom()
    {
        if(Input.mouseScrollDelta.y > 0f || Input.GetKey(KeyCode.C)) {
            targetOffset.y -= zoomAmount;
        } else if (Input.mouseScrollDelta.y < 0f || Input.GetKey(KeyCode.V)) {
            targetOffset.y += zoomAmount;
        }
        //Debug.Log(targetOffset.y);

        //make sure y does go above or below the limit
        targetOffset.y = Mathf.Clamp(targetOffset.y, CAMERA_OFFSET_MIN, CAMERA_OFFSET_MAX);

        cinemachineTransposerCamera.m_FollowOffset = 
        Vector3.Lerp(cinemachineTransposerCamera.m_FollowOffset, 
        targetOffset, Time.deltaTime * smoothfactor);


       // Debug.Log(cinemachineTransposerCamera.m_FollowOffset);
    }
}
