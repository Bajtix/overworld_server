using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Seat driverSeat;
    public Seat[] passengerSeats;

    public WheelCollider[] cmotorWheels;
    public WheelCollider[] cturningWheels;
    

    public Transform seatPos;

    public bool steered = false;

    public float maxRpm;
    public float torqe;
    public float brake;
    public float turnAngle;

    public bool debugMode = false;

    private bool dbg = false;

    private Entity entity;


    public List<WheelCollider> allWheels;

    private float[] r_rpms;
    private float[] r_pos;
    private sbyte[] r_turns;

    public float radiusOffset;

    private LayerMask wheelMask;

    private void Start()
    {
        entity = GetComponent<Entity>();

        

        //"Sums up" all the wheels.

        foreach(WheelCollider w in cmotorWheels)
        {
            allWheels.Add(w);
        }

        foreach (WheelCollider w in cturningWheels)
        {
            if(!allWheels.Contains(w))
                allWheels.Add(w);
        }

        //Initiates the arrays for rendering

        r_rpms = new float[allWheels.Count];
        r_pos = new float[allWheels.Count];
        r_turns = new sbyte[allWheels.Count];

        wheelMask = _GameUtilityToolset.GetPhysicsLayerMask(allWheels[0].gameObject.layer);
    }

    private void ApplyEmptyBrake()
    {
        if (!debugMode)
        {
            if (driverSeat.controller == null)
            {
                foreach (WheelCollider w in cmotorWheels)
                {
                    w.brakeTorque = 10000;
                }
                return;
            }
        }
        else
            foreach (WheelCollider w in cmotorWheels)
            {
                w.brakeTorque = 0;
            }
    }
    private void ApplyCorrectTorqe(WheelCollider w, float arpm)
    {
        if (arpm < maxRpm)
        {
            w.motorTorque = -torqe * driverSeat.vertical;
        }
        else
        {
            w.motorTorque = 0f;
        }
    }
    private float GetWheelY(WheelCollider w)
    {
        RaycastHit hit;
        Physics.Raycast(w.transform.position + w.center, Vector3.down, out hit, w.radius + w.suspensionDistance + radiusOffset + 1f, wheelMask);

        if (hit.collider != null)
        {
            Debug.Log(hit.distance);
            return hit.distance - w.radius - radiusOffset;
        }
        else
            return w.suspensionDistance;
    }

    private void RenderWheels()
    {
        for(int i = 0; i < allWheels.Count; i++)
        {
            r_rpms[i] = (-allWheels[i].rpm / 60) * 360;
            r_pos[i] = GetWheelY(allWheels[i]);
            r_turns[i] = (sbyte)Mathf.RoundToInt(allWheels[i].steerAngle);         
        }

        entity.additionalDataObject = (r_rpms, r_pos, r_turns);
    }

    private void FixedUpdate()
    {
        ApplyEmptyBrake();

        dbg = false;
        foreach (WheelCollider w in cmotorWheels)
        {
            float rpm = -w.rpm;
            float arpm = Mathf.Abs(rpm);
            float nrpm;

            if (rpm > 7f)
                nrpm = 0.01f;
            else if (rpm < -7f)
                nrpm = -0.01f;
            else
                nrpm = 0f;

            float nv = Mathf.Clamp(driverSeat.vertical, -0.01f, 0.01f);
            float av = Mathf.Abs(driverSeat.vertical);



            if (arpm > 0.01f || av > 0.01f)
            {
                if ((nv > nrpm || nv < nrpm) && nrpm != 0)
                {
                    w.motorTorque = 0;
                    w.brakeTorque = brake;
                }
                else
                {
                    w.brakeTorque = 0f;
                    ApplyCorrectTorqe(w, arpm);
                }
            }
            else
            {
                w.brakeTorque = 0;
                w.motorTorque = 0;
            }

            
             if (!dbg && debugMode)  //debug stuff
             {
                 dbg = true;
                 Debug.Log($"RPM: {rpm:000000.0}; NRPM: {nrpm:0.00}; Torque: {w.motorTorque:000000.0}; Brake: {w.brakeTorque:000000.0}"); 
             }
        }
        foreach (WheelCollider w in cturningWheels)
        {
            w.steerAngle = driverSeat.horizontal * turnAngle;
        }

        RenderWheels();
    }

    
}
