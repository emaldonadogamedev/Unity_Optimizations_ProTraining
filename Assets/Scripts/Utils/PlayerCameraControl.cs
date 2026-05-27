// #define  _DEBUG_LOG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{
    public Transform playerRef;
    new private Camera camera;
    private int layerMask;
    private bool isMovingBackward = false;
    private float backwardTargetDis = 0;
    private float backwardLerpTimeInv = 0;
    
    public float camFov = 40.0f;
    public float refDisFromPlayer = 7.0f;
    public float backwardLerpTime = 1.0f;
    [Range(0.0005f, 0.001f)]
    public float moveDisThresh = 0.0005f;
#if _DEBUG_LOG
    public float currentDisFromPlayer;  
    #else
    private float currentDisFromPlayer;  // private
#endif
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Wall");
        backwardLerpTimeInv = 1.0f / backwardLerpTime;
    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateDisFromPlayer();

        if (isMovingBackward)
        {
            // currentDisFromPlayer is updated every frame
            #if _DEBUG_LOG
            float delta = currentDisFromPlayer - backwardTargetDis; //Always negative
            Debug.Log($"moving backward. delta:{delta}");
            #endif
            float dis = Mathf.Lerp(0, currentDisFromPlayer - backwardTargetDis,backwardLerpTimeInv * Time.deltaTime);
            transform.position += transform.forward * dis;
        }
    }

    private void UpdateDisFromPlayer()
    {
        isMovingBackward = false;
        
        RaycastHit hit;
        float dis = 0;  // Final distance to update
        currentDisFromPlayer = (transform.position - playerRef.position).magnitude;
        if (Physics.Raycast(playerRef.position, transform.forward * -1.0f, out hit, currentDisFromPlayer, layerMask))
        {
            float disPlayerToHit = hit.distance;
            if (currentDisFromPlayer > disPlayerToHit)
            {
                // Move forward this transform (camera ref)
                dis = disPlayerToHit > refDisFromPlayer ? refDisFromPlayer : disPlayerToHit;
                if(Mathf.Abs(currentDisFromPlayer - dis) > moveDisThresh)
                    MoveForward(currentDisFromPlayer, dis);
            }
        }
        else
        {
            if (Mathf.Abs(currentDisFromPlayer - refDisFromPlayer) > moveDisThresh)
            {
                if (currentDisFromPlayer > refDisFromPlayer)
                {
                    MoveForward(currentDisFromPlayer, refDisFromPlayer);
                }
                else if(currentDisFromPlayer < refDisFromPlayer)
                {
                    MoveBackward(currentDisFromPlayer, refDisFromPlayer);
                }           
            }
        }
    }

    private void MoveForward(float currentDis, float targetDis)
    {
        // Move forward immediately
        transform.position += transform.forward * (currentDis - targetDis);
        #if _DEBUG_LOG
        Debug.Log($"move forward. delta:{currentDis - targetDis}");
        #endif
    }

    private void MoveBackward(float currentDis, float targetDis)
    {
        // Use lerp
        isMovingBackward = true;
        backwardTargetDis = targetDis;
    }

    public void SetParent(Transform parent)
    {
        playerRef = parent;
        camera = transform.GetComponentInChildren<Camera>();
        if (camera == null)
        {
            Debug.LogError("Can't find the camera component");
            enabled = false;
        }

        camera.fieldOfView = camFov;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerRef != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, playerRef.position);
        }
    }
}
