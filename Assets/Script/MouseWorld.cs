using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [field: SerializeField] public Transform sphere {get; private set;}
    [SerializeField] private LayerMask mousePlaneLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, mousePlaneLayerMask)) {
            Debug.Log("hit");
            sphere.position = rayCastHit.point;
        }
    }
}
