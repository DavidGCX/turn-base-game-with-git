using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyShell : MonoBehaviour
{
    void Update()
    {
        Destroy(this.gameObject, 10f);
    }
}

