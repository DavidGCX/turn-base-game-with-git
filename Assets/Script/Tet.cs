using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tet : MonoBehaviour
{
    public GridSystem grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = new GridSystem(10, 10, 2);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(grid.GetGridPosition(MouseWorld.GetMousePosition()));
    }
}
