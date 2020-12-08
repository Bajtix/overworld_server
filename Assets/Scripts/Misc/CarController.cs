using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Seat driverSeat;
    public Seat[] passengerSeats;

    public WheelCollider[] motorWheels;
    public WheelCollider[] turningWheels;
    public WheelCollider[] oppositeTurningWheels;


    public Transform seatPos;
    private Rigidbody rb;

    public bool steered = false;

    public float speed;
    public float brake = 1000;
    public float turnAngle;

    [Serializable]
    public struct Clutch
    {
        public float torque;

        public float minRpm;
        public float maxRpm;
    }

    public List<Clutch> clutches;
    public int clutch = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    [Serializable]
    private struct CarData
    {
        public float steerAngle;
        public float rpm;
        public bool steered;

        public CarData(float steerAngle, float rpm, bool steered)
        {
            this.steerAngle = steerAngle;
            this.rpm = rpm;
            this.steered = steered;
        }
    }

    private void FixedUpdate()
    {
        
        steered = driverSeat.controller != null;
        
        GetComponent<Entity>().additionalDataObject = new CarData(driverSeat.horizontal * turnAngle, motorWheels[0].rpm,steered);

        if (!steered)
        {
            rb.drag = 1;
            foreach (WheelCollider wheel in motorWheels)
            {
                wheel.brakeTorque = brake;
            }
        }
        else
        {
            rb.drag = 0.1f;
            if(driverSeat.vertical == 0 )
                foreach (WheelCollider wheel in motorWheels)
                {
                    wheel.brakeTorque = .7f;
                    wheel.motorTorque = 0f;
                }
            else
                foreach (WheelCollider wheel in motorWheels)
                {
                    wheel.brakeTorque = 0f;
                }
        }


        Clutch c = clutches[clutch];

        foreach (WheelCollider wheel in motorWheels)
        {
            if (wheel.rpm < c.maxRpm)
            {
                var trq = -c.torque * driverSeat.vertical;
                wheel.motorTorque = trq * Time.fixedDeltaTime * 30f;
                Debug.Log("applying torque: " + trq);
            }
            else if (clutch < clutches.Count - 1)
                clutch++;
            
            if (wheel.rpm < c.minRpm && clutch > 0)
                clutch--;

        }

        foreach (WheelCollider wheel in turningWheels)
        {
            wheel.steerAngle = turnAngle * driverSeat.horizontal;
        }

        foreach (WheelCollider wheel in oppositeTurningWheels)
        {
            wheel.steerAngle = turnAngle * -driverSeat.horizontal;
        }
    }
}
