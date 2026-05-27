using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamLightTween : MonoBehaviour
{
    public float loopDuration = 5.0f;
    public float maxIntensity = 5.0f;
    public DG.Tweening.Ease easeType;
    private Light thisLight;
    
    private Vector3 myVector = Vector3.zero;
    // Start is called before the first frame update
    
    void Start()
    {
        thisLight = transform.GetComponent<Light>();
        if (thisLight != null)
        {
            DOTween.To(() => thisLight.intensity, x => thisLight.intensity = x, maxIntensity, loopDuration)
                .SetEase(easeType).SetLoops(-1, LoopType.Yoyo);
        }
    }


}
