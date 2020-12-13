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


    private void FixedUpdate()
    {

        GetComponent<Entity>().additionalDataObject = (cturningWheels[0].steerAngle, cturningWheels[0].rpm, false);

        if (driverSeat.controller == null)
        {
            foreach (WheelCollider w in cmotorWheels)
            {
                w.brakeTorque = 10000;
            }
            return;
        }

        
       
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
                    WTorque(w, arpm);
                }
            }
            else
            {
                w.brakeTorque = 0;
                w.motorTorque = 0;
            }


            /* if (!dbg)  debug stuff
             {
                 dbg = true;
                 Debug.Log($"RPM: {rpm:000000.0}; NRPM: {nrpm:0.00}; Torque: {w.motorTorque:000000.0}; Brake: {w.brakeTorque:000000.0}"); 
             }*/
        }


        foreach (WheelCollider w in cturningWheels)
        {
            w.steerAngle = driverSeat.horizontal * turnAngle;
        }

    }

    private void WTorque(WheelCollider w, float arpm)
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
}
