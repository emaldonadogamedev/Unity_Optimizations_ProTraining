using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [ExecuteInEditMode]
public class ModifyVertTest : MonoBehaviour
{
    public Mesh mesh;
    private List<Vector3> verts;
    private List<Vector2> uvs;
    public bool modify = false;
    public int vertCnt = 0;
    public float length = 4.0f;
    public float randomMax = 0.1f;
    [Range(0.3f, 1)] public float delta = 1.0f;
    private float elapsedTime = 0;
    public int idxToModify = 0;

    private void Start()
    {
        this.mesh = transform.GetComponent<MeshFilter>().mesh;
        vertCnt = this.mesh.vertexCount;
        verts = new List<Vector3>(vertCnt);
        uvs = new List<Vector2>(vertCnt);
    }

    void Update()
    {
        if (modify)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 0.1f)
            {
                ModifyVerts();
                elapsedTime = 0;
            }
        }
        else
        {
            elapsedTime = 0;
        }
    }
    
    public void ModifyVerts()
    {
        mesh.GetVertices(verts);
        // mesh.GetUVs(0, uvs);
        Vector3 localPivot = verts[0];
        for (int i = 1; i < vertCnt; i++)
        {
            Vector3 normalizedDir = (verts[i] - localPivot).normalized;
            verts[i] = localPivot + normalizedDir * (length + Random.Range(-randomMax, randomMax));
        }
        mesh.SetVertices(verts);
        // mesh.SetUVs(0, uvs);
    }

    public void ModifyVert()
    {
        int idx = this.idxToModify;
        mesh.GetVertices(verts);
        mesh.GetUVs(0, uvs);
        Vector3 localPivot = verts[0];
        Vector3 targetVert = verts[idx];
        Vector3 normalizedDir = (targetVert - localPivot).normalized;
        float newLength = 4.190763f * delta;
        verts[idx] = localPivot + (normalizedDir * newLength);
        Vector2 oldUV = uvs[idx];
        float newUV = newLength * (0.7f - 0.1f) / 4.190763f;
        newUV = Mathf.Clamp(0.7f - newUV, 0.1f, 0.7f);
        uvs[idx] = new Vector2(oldUV.x, newUV);
        Debug.Log($"new uv y:{newUV}");
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
    }
}


#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(ModifyVertTest))]
public class ModifyVertTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Test"))
        {
            bool modify = ((ModifyVertTest)target).modify;
            ((ModifyVertTest)target).modify = !modify;
        }
        
        if (GUILayout.Button("Test2"))
        {
            ((ModifyVertTest)target).ModifyVert();
        }
    }
}
#endif

