using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomMessage : MonoBehaviour
{
    private float time;
    private bool active;
    private void Start() {
        time = Time.fixedTime;
        //Debug.Log("Start Called");
        active = false;
    }

    private void Update() {
        if(active) {
            time = Time.fixedTime;
            active = false;
        }
        if (Time.fixedTime - time > 1.5f) {
            active = true;
            transform.gameObject.SetActive(false);
        }
    }
}
