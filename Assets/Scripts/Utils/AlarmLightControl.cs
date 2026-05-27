using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class AlarmLightControl : MonoBehaviour
{
   // Light cone info
   private float refOffset = 4.0f;
   private float refRadius = 1.25f;
   private float refHypotenuse;
   private float refHypotenuseInv;

   private int layerMask;
   private Vector3[] rayDirections = new Vector3[5];

   public Transform lightCone;
   public Light spotLight;  
   public bool modifyCone = false;
   public bool modifyLightRange = false;
   public bool useAverageDis = true;
   [Range(0, 0.25f)] public float margin = 0.1f;
   public bool debugDrawRay = false;

   private void Awake()
   {
      layerMask = 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Default");
      refHypotenuse = Mathf.Sqrt((refOffset * refOffset) + (refRadius * refRadius));
      refHypotenuseInv = 1.0f / refHypotenuse;
      if (lightCone == null)
          lightCone = transform.Find("LightCone");
   }

   private void OnTriggerEnter(Collider other)
   {
       // Debug.Log($"OnTriggerEnter {other.gameObject.name}, {other.transform.position} frame:{Time.frameCount}");
       UpdateCone(FindClosestHitPoint());
   }

   private void OnTriggerStay(Collider other)
   {
       // Debug.Log($"OnTriggerStay {other.name} frame:{Time.frameCount}");
       UpdateCone(FindClosestHitPoint());
   }

   private void OnTriggerExit(Collider other)
   {
       // Debug.Log($"OnTriggerExit {other.name} frame:{Time.frameCount}");
       UpdateCone(refHypotenuse);
   }

   private float FindClosestHitPoint()
   { 
       if (!modifyCone) return refHypotenuse;
       
        // 5 rays
        Vector3 rayOrigin = transform.position;
        // Vector3 forward = transform.forward;
        Vector3 up = transform.up;
        Vector3 right = transform.right;
        Vector3 rayDirForward = transform.forward * refOffset;
        Vector3 rayDirUp = rayDirForward + up * refRadius;
        Vector3 rayDirDown = rayDirForward - up * refRadius;
        Vector3 rayDirRight = rayDirForward + right * refRadius;
        Vector3 rayDirLeft = rayDirForward - right * refRadius;

        rayDirections[0] = rayDirForward;
        rayDirections[1] = rayDirUp;
        rayDirections[2] = rayDirDown;
        rayDirections[3] = rayDirRight;
        rayDirections[4] = rayDirLeft;

        RaycastHit hit;
        bool didHit = false;
        float shortestDis = float.MaxValue;
        float averageDis = 0;
        // Vector3 shortestPoint = Vector3.zero;
        for (int i = 0; i < rayDirections.Length; i++)
        {
            if (Physics.Raycast(rayOrigin, rayDirections[i], out hit, refHypotenuse, layerMask))
            {
                DrawRay(rayOrigin, rayDirections[i], hit.distance, Color.red);
                didHit = true;
                averageDis += hit.distance;
                if (shortestDis > hit.distance)
                {
                    shortestDis = hit.distance;
                    // shortestPoint = hit.point;
                }
            }
            else
            {
                averageDis += refHypotenuse;
                DrawRay(rayOrigin, rayDirections[i], refHypotenuse, Color.blue);
            }
        }

        // Debug.Log($"Shortest dis:{shortestDis}, point:{shortestPoint}, didHit:{didHit}");
        if (useAverageDis)
            return averageDis * 0.2f;
        else
            return didHit ? shortestDis : refHypotenuse;
   }

    private void UpdateCone(float dis)
    {
        float newScale = dis * refHypotenuseInv;
        newScale = Mathf.Clamp(newScale * (1.0f + margin), 0.1f, 1);
        // Debug.Log($"ratio:{dis/refHypotenuse}");
        lightCone.localScale = new Vector3(newScale, newScale, newScale);

        spotLight.range = modifyLightRange ? Mathf.Clamp(dis, 0, refOffset) : refOffset;
    }

    private void DrawRay(Vector3 start, Vector3 unnormalizedDirection, float length, Color color, float duration = .1f)
    {
        if (debugDrawRay)
            DrawRay(start, start + unnormalizedDirection.normalized * length, color, duration);
    }
    
    private void DrawRay(Vector3 start, Vector3 end, Color color, float duration = 0.1f)
    {
        if(debugDrawRay)
            Debug.DrawRay(start, end - start, color, duration);
    }
}
