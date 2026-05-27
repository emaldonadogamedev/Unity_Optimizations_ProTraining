using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ModulePrefabManager : MonoBehaviour
{
    public Collider[] colliders;
    public float minY = 0.5f;

    public Material[] wallMaterials;
    public NavMeshObstacleSizeContainer sizeContainer;

    public void Awake()
    {
        SwapWallMaterial();
    }

    public void SwapWallMaterial()
    {
        int idx = Random.Range(0, wallMaterials.Length);
        MeshRenderer[] mrs = transform.Find("Walls").GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            int cnt = mr.sharedMaterials.Length;
            if(cnt == 1)
                mr.sharedMaterial = wallMaterials[idx];
            else
            {
                for (int i = 0; i < cnt; i++)
                   mr.sharedMaterials[i] = wallMaterials[idx];
            }
        }
    }
    public void AddNavMesh()
    {
        colliders = transform.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            // Exception rules
            if(col.transform.GetComponent<Rigidbody>()) continue;
            
            float minY = col.bounds.center.y - col.bounds.extents.y;
            if (minY < this.minY)
            {
                NavMeshObstacle nav = col.gameObject.GetComponent<NavMeshObstacle>();
                if (nav == null)
                {
                    nav = col.gameObject.AddComponent<NavMeshObstacle>();
                    nav.carving = true;
                    Debug.Log($"{col.name} y:{minY}");
                    if (col.GetType().Equals(typeof(MeshCollider)))
                    {
                        // NavMeshObstacle
                        // Debug.Log($"{col.name}'s collider is MeshCollider");
                        // Debug.Log(col.bounds);
                        // Debug.Log(col.transform.position);
                        /*
                        Vector3 offset = col.bounds.center - col.transform.position;
                        nav.center = new Vector3(offset.x / col.transform.localScale.x,
                                                    offset.y / col.transform.localScale.y, 
                                                    offset.z / col.transform.localScale.z);
                        Vector3 size = col.bounds.extents * 2.0f;
                        nav.size = new Vector3(size.x / col.transform.localScale.x,
                                                size.y / col.transform.localScale.y, 
                                                size.z / col.transform.localScale.z);
                        */
                        Vector3 center, size;
                        sizeContainer.GetInfo(col.transform.name, out center, out size);
                        nav.center = center;
                        nav.size = size;
                    }
                    else if (col.GetType().Equals(typeof(BoxCollider)))
                    {
                        // BoxCollider 
                        Debug.Log($"BoxCollider center:{((BoxCollider)col).center}, size:{((BoxCollider)col).size}");
                        Debug.Log($"NavMeshObstacle center:{nav.center}, size:{nav.size}");
                        nav.center = ((BoxCollider)col).center;
                        nav.size = ((BoxCollider)col).size;
                    }
                }
            }
        }
    }

    public void RemoveNavMesh()
    {
        colliders = transform.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            NavMeshObstacle nav = col.transform.GetComponent<NavMeshObstacle>();
            if (nav != null)
            {
                if (Application.isPlaying)
                    GameObject.Destroy(nav);
                else
                    GameObject.DestroyImmediate(nav);
            }
        }
    }

    public void Test(string name)
    {
        Vector3 a, b;
        sizeContainer.GetInfo(name, out a, out b);
        Debug.Log($"a:{a}, b:{b}");
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(ModulePrefabManager))]
public class ModulePrefabManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        GUILayout.Label("NavMeshObstacle");
        if (GUILayout.Button("Add"))
        {
            ((ModulePrefabManager)target).AddNavMesh();
        }
        if (GUILayout.Button("Remove"))
        {
            ((ModulePrefabManager)target).RemoveNavMesh();
        }
        GUILayout.Space(10);
        GUILayout.Label("Wall Material");
        if (GUILayout.Button("Swap"))
        {
            ((ModulePrefabManager)target).SwapWallMaterial();
        }

        // GUILayout.Space(10);
        // if (GUILayout.Button("Test"))
        // {
        //     ((ModulePrefabManager)target).Test("sm_shelfa");
        // }
    }
}
#endif
