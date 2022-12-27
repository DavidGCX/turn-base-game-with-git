using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{

    [SerializeField] private MeshRenderer Quad;

    [SerializeField] private MeshRenderer EnemyQuad;


    // Start is called before the first frame update

    public void Show() {
        Quad.enabled = true;
    }

    public void Hide() {
        Quad.enabled = false;
        EnemyQuad.enabled = false;
    } 

    public void ShowtAttackTarget() {
        EnemyQuad.enabled = true;
    }
}
