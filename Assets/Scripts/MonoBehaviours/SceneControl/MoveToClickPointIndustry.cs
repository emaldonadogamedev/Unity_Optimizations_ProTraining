using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPointIndustry : MoveToClickPoint
{   
    [SerializeField]
    private Color colorMove, colorStationary;

    [SerializeField]
    private float stopVelThresh = 0.05f;

    private Light indicatorLight;
    private NavMeshAgent navMeshAgent;


    private void Awake()
    {
        Transform cameraRef = transform.Find("CameraRef");
        
        GameObject mainCam = GameObject.FindWithTag("MainCamera");
        mainCam.transform.SetParent(cameraRef);
        mainCam.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        mainCam.transform.localScale = Vector3.one;
 
        PlayerCameraControl ctrl = cameraRef.gameObject.GetComponent<PlayerCameraControl>();
        if(ctrl == null)
            ctrl = cameraRef.gameObject.AddComponent<PlayerCameraControl>();
        ctrl.SetParent(transform);

        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        indicatorLight = transform.GetComponentInChildren<Light>();
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

        if (IsMoving())
        {
            Debug.Log("Roomba is moving");
            indicatorLight.color = colorMove;
        }
        else
            indicatorLight.color = colorStationary;
    }

    private bool IsMoving()
    {
        return navMeshAgent.velocity.sqrMagnitude > stopVelThresh;
    }
}
