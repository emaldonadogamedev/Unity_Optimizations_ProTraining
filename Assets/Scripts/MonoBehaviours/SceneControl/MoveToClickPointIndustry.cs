using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPointIndustry : MoveToClickPoint
{   
    [SerializeField]
    private Color colorMove, colorStationary;

    [SerializeField]
    private float stopVelThresh = 0.05f;

    private Light indicatorLight;


    private void Awake()
    {
        
    }

    protected override void Start()
    {
        base.Start();
        
        SetMainCamera();

        indicatorLight = transform.GetComponentInChildren<Light>();
    }

    private void SetMainCamera()
    {
        Transform cameraRef = transform.Find("CameraRef");

        GameObject mainCam = GameObject.FindWithTag("MainCamera");
        mainCam.transform.SetParent(cameraRef);
        mainCam.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        mainCam.transform.localScale = Vector3.one;

        PlayerCameraControl ctrl = cameraRef.gameObject.GetComponent<PlayerCameraControl>();
        if (ctrl == null)
            ctrl = cameraRef.gameObject.AddComponent<PlayerCameraControl>();
        ctrl.SetParent(transform);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        CheckIndicatorLight();
    }

    private void CheckIndicatorLight()
    {
        if (indicatorLight == null)
            return;

        indicatorLight.color = IsMoving() ? colorMove : colorStationary;
    }

    private bool IsMoving()
    {
        return agent.velocity.sqrMagnitude > stopVelThresh;
    }
}
