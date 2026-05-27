using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class OpenConeMeshBuilder : MonoBehaviour
{
    public float baseRadius;

    public int sides;

    public float vertexOffset;
    public Vector2 pivotUV;
    public Vector2 uvStart, uvEnd;
    public bool optimizeMesh = false;
    
    [HideInInspector]
    public MeshFilter mf;
    private MeshRenderer mr;
    private void CheckComponents()
    {
        MeshFilter mf = transform.GetComponent<MeshFilter>();
        if (mf == null)
            this.mf = gameObject.AddComponent<MeshFilter>();
        else
            this.mf = mf;

        MeshRenderer mr = transform.GetComponent<MeshRenderer>();
        if (mr == null)
            this.mr = gameObject.AddComponent<MeshRenderer>();
        else
            this.mr = mr;
    }

    public void Build()
    {
        CheckComponents();
     
        Vector3[] verts = new Vector3[1 + sides];
        Vector2[] uvs = new Vector2[1 + sides];
        
        Vector3 pivot = transform.position;
        verts[0] = pivot;
        uvs[0] = pivotUV;
        
        // Vector3 coneCenter = pivot + Vector3.down * vertexOffset;
        Vector3 coneCenter = pivot + Vector3.forward * vertexOffset;
        // Vector3[] verticesOnCircle = new Vector3[sides];
        float rotDelta = 360.0f / (float)sides;

        // Vector3 testScale = new Vector3(0.1f, 0.1f, 0.1f);
        Vector3 lineVec = Vector3.right * baseRadius;
        Vector2 uvDelta = uvEnd - uvStart;
        float uvMul = 1.0f / (float)(sides - 1);
        for (int i = 0; i < sides; i++)
        {
            Quaternion rot = Quaternion.AngleAxis(rotDelta * (float)i, Vector3.back);
            Vector3 pos = coneCenter + rot * lineVec;
            // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // cube.transform.position = pos;
            // cube.transform.localScale = testScale;
            //
            // verticesOnCircle[i] = pos;
            verts[i + 1] = pos;

            uvs[i + 1] = uvStart + uvDelta * ((float)i * uvMul);
        }

        for (int i = 0; i < verts.Length; i++)
            verts[i] -= pivot;

        // Mesh Build
        Mesh mesh = new Mesh();
        
        // Vertices
        mesh.vertices = verts;
        
        // Triangles
        int[] tris = new int[3 * sides];
        for (int i = 0; i < sides; i++)
        {
            tris[i * 3] = 0;
            tris[i * 3 + 1] = i + 1;
            int idx = i + 2;
            if (idx > sides)    idx -= sides;
            tris[i * 3 + 2] = idx;
        }
        mesh.triangles = tris;
        
        // UVs
        mesh.uv = uvs;
        Debug.Log("UV");
        for(int i = 0; i < uvs.Length; i++)
            Debug.Log($"idx:{i}, uv:{uvs[i]}");
        
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mf.sharedMesh = mesh;
    }
    
    public void BuildClosedMesh()
    {
        CheckComponents();
     
        Vector3[] verts = new Vector3[1 + sides + 1];
        Vector2[] uvs = new Vector2[1 + sides + 1];
        
        Vector3 pivot = transform.position;
        verts[0] = pivot;
        uvs[0] = pivotUV;
        
        Vector3 coneCenter = pivot + Vector3.down * vertexOffset;
        // Vector3[] verticesOnCircle = new Vector3[sides];
        float rotDelta = 360.0f / (float)sides;

        // Vector3 testScale = new Vector3(0.1f, 0.1f, 0.1f);
        Vector3 lineVec = Vector3.forward * baseRadius;
        Vector2 uvDelta = uvEnd - uvStart;
        float uvMul = 1.0f / (float)(sides - 1);
        for (int i = 0; i < sides; i++)
        {
            Quaternion rot = Quaternion.AngleAxis(rotDelta * (float)i, Vector3.up);
            Vector3 pos = coneCenter + rot * lineVec;
            // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // cube.transform.position = pos;
            // cube.transform.localScale = testScale;
            //
            // verticesOnCircle[i] = pos;
            verts[i + 1] = pos;

            uvs[i + 1] = uvStart + uvDelta * ((float)i * uvMul);
        }

        verts[verts.Length - 1] = coneCenter;
        uvs[uvs.Length - 1] = pivotUV;

        for (int i = 0; i < verts.Length; i++)
            verts[i] -= pivot;

        // Mesh Build
        Mesh mesh = new Mesh();
        
        // Vertices
        mesh.vertices = verts;
        
        // Triangles
        int[] tris = new int[3 * sides * 2];
        for (int i = 0; i < sides; i++)
        {
            tris[i * 3] = 0;
            tris[i * 3 + 1] = i + 1;
            int idx = i + 2;
            if (idx > sides)    idx -= sides;
            tris[i * 3 + 2] = idx;
        }

        int idxOffset = 3 * sides;
        for (int i = 0; i < sides; i++)
        {
            tris[idxOffset + (i * 3)] = verts.Length - 1;
            int idx = i + 2;
            if (idx > sides) idx -= sides;
            tris[idxOffset + (i * 3 + 1)] = idx;
            tris[idxOffset + (i * 3 + 2)] = i + 1;
        }
        
        mesh.triangles = tris;
        
        // UVs
        mesh.uv = uvs;
        Debug.Log("UV");
        for(int i = 0; i < uvs.Length; i++)
            Debug.Log($"idx:{i}, uv:{uvs[i]}");
        
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mf.sharedMesh = mesh;
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(OpenConeMeshBuilder))]
public class OpenConeMeshBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Build"))
        {
            ((OpenConeMeshBuilder)target).Build();
        }
        
        if(GUILayout.Button("Build Closed Mesh"))
        {
            ((OpenConeMeshBuilder)target).BuildClosedMesh();
        }

        if (GUILayout.Button("Save Mesh"))
        {
            string path = EditorUtility.SaveFilePanel("Save Mesh Asset", "Assets/", name, "asset");
            if (string.IsNullOrEmpty(path)) return;
        
            path = FileUtil.GetProjectRelativePath(path);

            Mesh meshToSave = ((OpenConeMeshBuilder)target).mf.sharedMesh;
		
            if (((OpenConeMeshBuilder)target).optimizeMesh)
                MeshUtility.Optimize(meshToSave);
        
            AssetDatabase.CreateAsset(meshToSave, path);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif
