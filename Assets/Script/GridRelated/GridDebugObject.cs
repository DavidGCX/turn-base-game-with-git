using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private GridObject gridObject;

    public void SetGridDebugObject(GridObject gridObject) {
        this.gridObject = gridObject;
    }

    public override string ToString()
    {
        return gridObject.ToString();
    }

    private void Update() {
        text.text = gridObject.ToString();    
    }
}
