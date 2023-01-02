using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellGenerate : MonoBehaviour
{
    [SerializeField] Transform shellSpawningPlace;

    [SerializeField] Transform shellPrefab;

    float force = 2000f;
    float UpDownRate = 40f; //弹出速度上下浮动20%

    private void OnEnable() {
       
        Transform shell = Instantiate(shellPrefab, shellSpawningPlace);
        float resultForce = force * (1 - UnityEngine.Random.Range(-UpDownRate, UpDownRate)/ 100);
        shell.GetComponent<Rigidbody>().AddForce(shell.right * resultForce);
        shell.GetComponent<Rigidbody>().AddTorque(-shell.forward * resultForce);
        shell.SetParent(null);
    }
}
