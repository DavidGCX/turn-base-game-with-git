using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance {get; private set;}
    CinemachineImpulseSource cinemachineImpulseSource;
    // Start is called before the first frame update
    void Start()
    {
        
        Instance = this;
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    public void Shake(float intensity = 0.3f) {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
