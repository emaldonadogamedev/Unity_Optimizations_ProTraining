using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CautionStripMeshModifier : MonoBehaviour
{
    public float RectOffset = 0;

    public void ElevateSubmesh()
    {
        Mesh mesh = transform.GetComponent<MeshFilter>().sharedMesh;
        
        // mesh.
    }
}


