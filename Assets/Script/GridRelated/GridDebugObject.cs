using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GridDebugObject: MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private object gridObject;

    public virtual void SetGridDebugObject(object gridObject) {
        this.gridObject = gridObject;
    }

    protected virtual void Update() {
        text.text = gridObject.ToString();    
    }
}
