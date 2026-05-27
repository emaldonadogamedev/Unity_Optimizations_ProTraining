using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPointIndustry : MoveToClickPoint
{
    private Light indicatorLight;
    
    [SerializeField]
    private Color colorMove, colorStationary;

    [SerializeField] private float stopVelThresh = 0.05f;


    private void Awake() {
        Transform cameraRef = transform.Find("CameraRef");
        GameObject mainCam = GameObject.FindWithTag("MainCamera");
        mainCam.transform.SetParent(cameraRef);
        mainCam.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        mainCam.transform.localScale = Vector3.one;
        PlayerCameraControl ctrl = cameraRef.gameObject.GetComponent<PlayerCameraControl>();
        if(ctrl == null)
            ctrl = cameraRef.gameObject.AddComponent<PlayerCameraControl>();
        ctrl.SetParent(transform);
    }

    protected override void Start()
    {
        base.Start();
        indicatorLight = transform.GetComponentInChildren<Light>();
    }
    protected override void Update()
    {
        base.Update();
        CheckIndicatorLight();
    }
    
    private void CheckIndicatorLight()
    {
        if (indicatorLight != null){
            {
                if (IsMoving()){
                    indicatorLight.color = colorMove;
                }
                else indicatorLight.color = colorStationary;
            }
        }
            
    }

    private bool IsMoving()
    {
        Debug.Log("Roomba is moving");
        return GetComponent<NavMeshAgent>().velocity.sqrMagnitude > stopVelThresh;
    }

    
}
