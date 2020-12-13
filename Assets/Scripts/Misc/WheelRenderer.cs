using NWH.WheelController3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRenderer : MonoBehaviour
{
    public WheelCollider wheel;

    
    private float motorAngle = 0;
    private Vector3 basePos;

    private void Start()
    {
        basePos = transform.localPosition;
    }
    private void Update()
    {
        motorAngle += (wheel.rpm / 60) * 360 * Time.deltaTime;
        transform.localRotation = Quaternion.AngleAxis(wheel.steerAngle + 90, Vector3.up) * Quaternion.AngleAxis(motorAngle, Vector3.forward);
        
    }
}
