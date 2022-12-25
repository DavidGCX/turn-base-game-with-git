using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraUI : MonoBehaviour
{
    Transform cameraPos;
    // Start is called before the first frame update
    void Start()
    {
        cameraPos = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(cameraPos.position.x, 0f, cameraPos.position.z);
        transform.LookAt(pos);
    }
}
