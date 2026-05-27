// https://www.youtube.com/watch?v=xFONZZMWxD0

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RotateLights : MonoBehaviour
{
    public float rotSpeed = 30.0f;  // deg per sec
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody thisRigidbody = transform.GetComponent<Rigidbody>();
        if (thisRigidbody != null)
        {
            thisRigidbody.DORotate(new Vector3(0, 360, 0), 360.0f / rotSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetRelative()
                .SetEase(Ease.Linear);
        }
        else
        {
            transform.DORotate(new Vector3(0, 360, 0), 360.0f / rotSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetRelative()
                .SetEase(Ease.Linear);    
        }
    }
}
