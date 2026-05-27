using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CollisionTest : MonoBehaviour
{
    private int layerMask;
    private float radius;
    public Transform rotateParent;
    public Transform testPlane;

    private float refOffset = 4.0f;
    private float refRadius = 1.25f;
    public float refHypotenuse = 0;
    private ContactPoint[] contactPoints = new ContactPoint[20];

    private Vector3 rayBase;
    public Transform marker;
    public Transform lightCone;
    public bool debugDrawRay = false;
    [Range(0, 0.25f)]
    public float margin = 0.1f;
    private void Awake()
    {
        layerMask = 1 << LayerMask.NameToLayer("Default");
        // radius = transform.GetComponent<SphereCollider>().radius;

        refHypotenuse = Mathf.Sqrt((refOffset * refOffset) + (refRadius * refRadius));
        // testPlane.position = transform.position - transform.up * 2.0f;

        marker.position = Vector3.zero;

        // transform.GetComponent<MeshFilter>().mesh = new Mesh();
        // Mesh mesh = transform.GetComponent<MeshFilter>().sharedMesh;
        // mesh.SetVertices();
    }
    
    // void OnDrawGizmosSelected()
    // {
    //     Vector3 rayOrigin = transform.position;
    //     Vector3 rayPointUp = transform.position - transform.up * refOffset + transform.right * refRadius;
    //     Vector3 rayPointDown = transform.position - transform.up  * refOffset - transform.right * refRadius;
    //     Vector3 rayPointRight = transform.position - transform.up * refOffset + transform.forward * refRadius;
    //     Vector3 rayPointLeft = transform.position - transform.up * refOffset - transform.forward * refRadius;
    //     Debug.DrawRay(rayOrigin, (rayPointUp - rayOrigin), Color.white);
    //     Debug.DrawRay(rayOrigin, (rayPointDown - rayOrigin), Color.black);
    //     Debug.DrawRay(rayOrigin, (rayPointRight - rayOrigin), Color.red);
    //     Debug.DrawRay(rayOrigin, (rayPointLeft - rayOrigin), Color.blue);
    //     Debug.DrawRay(rayOrigin, -transform.up * refOffset, Color.magenta);
    // }

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log($"OnCollisionEnter {collision.contactCount}");
/*
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 hitPoint = new Vector3(collision.contacts[i].point.x, transform.position.y,
                collision.contacts[i].point.z);
            Debug.DrawLine(transform.position, hitPoint, Color.blue, 10.0f);
            Debug.Log($"length:{(transform.position - hitPoint).magnitude}");
        }
        // Debug.Break();
        */

        FindShortest(collision);
    }

    private void OnCollisionStay(Collision collisionInfo)
    {



        Debug.Log($"OnCollisionStay {collisionInfo.contactCount}");
/*
        for (int i = 0; i < collisionInfo.contactCount; i++)
        {
            Vector3 hitPoint = new Vector3(collisionInfo.contacts[i].point.x, transform.position.y,
                collisionInfo.contacts[i].point.z);
            Debug.DrawLine(transform.position, hitPoint, Color.cyan, 10.0f);
            Debug.Log($"length:{(transform.position - hitPoint).magnitude}");
        }
        // Debug.Break();
        */


        FindShortest(collisionInfo);
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log($"OnCollisionExit");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter {other.gameObject.name}");
        float dis = FindClosestHitPoint();
        UpdateCone(dis);
        // RaycastHit hit;
        // if (Physics.Raycast(rotateParent.position, transform.position - rotateParent.position, out hit, 100.0f, layerMask))
        // {
        //     float dis = hit.distance - (transform.position - rotateParent.position).magnitude;
        //     Debug.Log($"hit.distance:{hit.distance}, dis:{dis}");
        //     Debug.DrawLine(rotateParent.position, new Vector3(hit.point.x, rotateParent.position.y, hit.point.z), Color.blue, 10.0f);
        //     Debug.Break();
        //     // if (dis < radius)
        //     //     transform.position -= transform.forward * (radius - dis);
        // }
        // else
        // {
        //     Debug.Log("No hit");
        // }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay");
        float dis = FindClosestHitPoint();
        UpdateCone(dis);
    }

    private void OnTriggerExit(Collider collider)
    {
        Debug.Log("OnTriggerExit");
        marker.position = Vector3.zero;
        UpdateCone(refHypotenuse);
    }

    private float FindClosestHitPoint()
    {
        // 5 rays
        Vector3 rayOrigin = transform.position;
        Vector3 forward = -transform.up;
        Vector3 up = transform.right;
        Vector3 right = transform.forward;
        Vector3 rayDirForward = forward * refOffset;
        Vector3 rayDirUp = rayDirForward + up * refRadius;
        Vector3 rayDirDown = rayDirForward - up * refRadius;
        Vector3 rayDirRight = rayDirForward + right * refRadius;
        Vector3 rayDirLeft = rayDirForward - right * refRadius;

        int layerMask = 1 << LayerMask.NameToLayer("Wall");

        float shortestDis = float.MaxValue;
        Vector3 shortestPoint = Vector3.zero;
        Vector3[] rayDirections = new Vector3[5];
        rayDirections[0] = rayDirForward;
        rayDirections[1] = rayDirUp;
        rayDirections[2] = rayDirDown;
        rayDirections[3] = rayDirRight;
        rayDirections[4] = rayDirLeft;
        RaycastHit hit;
        bool didHit = false;
        for (int i = 0; i < rayDirections.Length; i++)
        {
            if (Physics.Raycast(rayOrigin, rayDirections[i], out hit, refHypotenuse, layerMask))
            {
                DrawRay(rayOrigin, rayOrigin + rayDirections[i].normalized * hit.distance, Color.red);
                didHit = true;
                if (shortestDis > hit.distance)
                {
                    shortestDis = hit.distance;
                    shortestPoint = hit.point;
                }
            }
            else
            {
                DrawRay(rayOrigin, rayOrigin + rayDirections[i].normalized * refHypotenuse, Color.blue);
            }
        }

        Debug.Log($"Shortest dis:{shortestDis}, point:{shortestPoint}");
        if (didHit)
        {
            marker.position = shortestPoint;
            return shortestDis;
        }
        else
        {
            marker.position = Vector3.zero;
            return refHypotenuse;
        }
    }

    private void UpdateCone(float dis)
    {
        float newScale = dis / refHypotenuse;
        newScale *= (1.0f + margin);
        newScale = Mathf.Clamp(newScale, 0, 1);
        Debug.Log($"ratio:{dis/refHypotenuse}");
        lightCone.localScale = new Vector3(newScale, newScale, newScale);
    }

    private void DrawRay(Vector3 start, Vector3 end, Color color, float duration = 0.1f)
    {
        if(debugDrawRay)
            Debug.DrawRay(start, end - start, color, duration);
    }

    private void FindShortest(Collision collisionInfo)
    {
        float shortest = float.MaxValue;
        int cnt = collisionInfo.GetContacts(contactPoints);
        Debug.Log($"contacts:{collisionInfo.contactCount}");
        for (int i = 0; i < cnt; i++)
        {
            Vector3 hitPoint = contactPoints[i].point;
            Debug.Log($"hit point:{hitPoint}, separation:{contactPoints[i].separation}");
            Debug.DrawLine(transform.position, hitPoint, Color.blue, 10.0f);
            float dis = (transform.position - hitPoint).magnitude;
            // refHypotenuse : dis = refOffset : offset;
            float offset = dis * refOffset / refHypotenuse;
            // Debug.Log($"Dis to hit point:{dis}, its offset:{offset}, {collisionInfo.contacts[i].thisCollider.name}");
            if (shortest > offset)
                shortest = offset;
        }
        
        // Debug.Log($"shortest:{shortest}");
        // float currentPlaneDis = (transform.position - testPlane.position).magnitude;
        if(testPlane != null)
            testPlane.position = transform.position - transform.up * shortest;
    }

   
}
