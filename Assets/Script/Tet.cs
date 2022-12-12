using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tet : MonoBehaviour
{
    
    [SerializeField] private Transform debugPrefab;
    public GridSystem grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = new GridSystem(10, 10, 2);
        grid.CreateDebugObject(debugPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(grid.GetGridPosition(MouseWorld.GetMousePosition()));
    }
}
