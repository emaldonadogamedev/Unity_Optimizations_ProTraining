using System;
using UnityEngine;
using System.Collections;

// Copy meshes from children into the parent's Mesh.
// CombineInstance stores the list of meshes.  These are combined
// and assigned to the attached Mesh.

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class MeshCombiner : MonoBehaviour
{
    public MeshFilter[] meshFilters;
    public float yOffset = 0;

    public void CheckOffset()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        Debug.Log($"Found {meshFilters.Length} MeshFilters");
        int i = 0;
        int len = 0;
        double x = 0, y = 0, z = 0;
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = minX, minZ = minX;
        float maxY = maxX, maxZ = maxX;
        while (i < meshFilters.Length)
        {
            Mesh mesh = meshFilters[i].sharedMesh;
            if (mesh == null){
                // Debug.LogError($"mesh is null {meshFilters[i].transform.parent.name}");
                Debug.LogError($"mesh is null {i}");
                i++;
                continue;
            }
            
            len += mesh.vertexCount;
            Transform thisTransform = meshFilters[i].transform;
            Debug.Log(thisTransform.localToWorldMatrix.ToString());
            foreach (Vector3 pos in mesh.vertices)
            {
                Vector3 worldPos = thisTransform.TransformPoint(pos);
                x += (double)worldPos.x;
                y += (double)worldPos.y;
                z += (double)worldPos.z;
                if (minX > worldPos.x) minX = worldPos.x;
                if (maxX < worldPos.x) maxX = worldPos.x;
                if (minY > worldPos.y) minY = worldPos.y;
                if (maxY < worldPos.y) maxY = worldPos.y;
                if (minZ > worldPos.z) minZ = worldPos.z;
                if (maxZ < worldPos.z) maxZ = worldPos.z;
            }
            i++;
        }
        Debug.Log($"len:{len}");
        x /= (double)len;
        y /= (double)len;
        z /= (double)len;
        Debug.Log($"Average x:{x}, y:{y}, z:{z}");
        Debug.Log($"Min:({minX}, {minY}, {minZ}), Max:({maxX}, {maxY}, {maxZ})");
        yOffset = minY + (maxY - minY) * 0.5f;
        this.meshFilters = meshFilters;
    }
    
    public void CombineMesh1()
    {
        // MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            // combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Matrix4x4 localToWorld = meshFilters[i].transform.localToWorldMatrix;
            localToWorld[1, 3] -= this.yOffset;
            combine[i].transform = localToWorld;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        transform.gameObject.SetActive(true);

        
    }
}