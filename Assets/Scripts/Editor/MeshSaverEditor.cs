using UnityEditor;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Timeline;

public static class MeshSaverEditor {

    [MenuItem("CONTEXT/MeshFilter/Save Mesh...")]
    public static void SaveMeshInPlace (MenuCommand menuCommand) {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        SaveMesh(m, m.name, false, true);
    }
    
    [MenuItem("CONTEXT/MeshFilter/Save Mesh(rm offset)...")]
    public static void SaveMeshInPlace2 (MenuCommand menuCommand) {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        SaveMesh(m, m.name, false, true, true);
    }
    
    [MenuItem("CONTEXT/MeshFilter/Save Roomba Mesh...")]
    public static void SaveMeshInPlaceRoomba (MenuCommand menuCommand) {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        SaveMeshRoomba(m, m.name, true, true);
    }

    [MenuItem("CONTEXT/MeshFilter/Save Mesh As New Instance...")]
    public static void SaveMeshNewInstanceItem (MenuCommand menuCommand) {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        SaveMesh(m, m.name, true, true);
    }
    

    public static void SaveMesh (Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh, bool removeOffset = false) {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;
        
        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = null;
        if (removeOffset)
        {
            Vector3 offset = Vector3.zero;
            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++)
            {
                offset += verts[i];
            }

            offset /= (float)verts.Length;
            Debug.Log(string.Format("Offset : {0:0.00000}, {1:0.0000}, {2:0.0000}", offset.x, offset.y, offset.z));

            meshToSave = new Mesh();
            for (int i = 0; i < verts.Length; i++)
            {
                verts[i] -= offset;
            }
            meshToSave.vertices = verts;
            meshToSave.triangles = mesh.triangles;
            meshToSave.uv = mesh.uv;
            meshToSave.uv2 = mesh.uv2;
            meshToSave.normals = mesh.normals;
            meshToSave.RecalculateBounds();
        }
        else
            meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

        if (meshToSave == null)
            return;
        
        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);
        
        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }
    
    public static void SaveMeshRoomba (Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh) {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;
        
        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = null;
        Vector3 offset = new Vector3(0, 0.012f, 0);
        Vector3[] verts = mesh.vertices;
        meshToSave = new Mesh();
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] += offset;
        }
        meshToSave.vertices = verts;
        meshToSave.triangles = mesh.triangles;
        meshToSave.uv = mesh.uv;
        meshToSave.uv2 = mesh.uv2;
        meshToSave.normals = mesh.normals;
        meshToSave.RecalculateBounds();
      
        if (meshToSave == null)
            return;
        
        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);
        
        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }
}