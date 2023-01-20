using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    void Start()
    {
        AttackAction.OnAttackActionCameraRequired += AttackAction_OnAttackStart;
        AttackAction.OnAttackComplete += AttackAction_OnAttackComplete;
        HideActionCamera();
    }

    private void AttackAction_OnAttackComplete()
    {
        HideActionCamera();
    }

    private void AttackAction_OnAttackStart(object sender, AttackAction.AttackActionCameraArgs e)
    {
        AttackAction attackAction = (AttackAction) sender;
        actionCameraGameObject.transform.SetParent(attackAction.gameObject.transform);
        actionCameraGameObject.transform.position = e.cameraPosition;
        actionCameraGameObject.transform.LookAt(e.cameraLookAtPosition);
        ShowActionCamera();
        Debug.Log("Camera Switch");
    }

    private void ShowActionCamera() {
         actionCameraGameObject.transform.SetParent(null);
        actionCameraGameObject.SetActive(true);
    }
    private void HideActionCamera() {
        actionCameraGameObject.SetActive(false);
    }
}
