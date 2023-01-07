using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{

    [SerializeField] private MeshRenderer Quad;



    // Start is called before the first frame update

    public void Show(Material material) {
        Quad.enabled = true;
        Quad.material = material;
    }

    public void Hide() {
        Quad.enabled = false;
    } 
}
