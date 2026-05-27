using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[CreateAssetMenu]
public class NavMeshObstacleSizeContainer : ScriptableObject
{
    [Serializable]
    public struct SizeData
    {
        public string name;
        public Vector3 center;
        public Vector3 size;
    }

    public SizeData[] dataList;

    public void GetInfo(string name, out Vector3 center, out Vector3 size)
    {
        foreach (SizeData data in dataList)
        {
            Debug.Log($"data.name:{data.name}, name:{name}");
            if(name.ToLower().Contains(data.name.ToLower(), StringComparison.Ordinal))
            {
                center = data.center;
                size = data.size;
                return;
            }
        }

        center = Vector3.zero;
        size = Vector3.zero;
    }
}
