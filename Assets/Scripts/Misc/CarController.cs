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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetComponent<Entity>().additionalData = (driverSeat.horizontal * turnAngle).ToString();
        steered = driverSeat.controller != null;
        if(!steered)
        {
            rb.drag = 3;
            foreach (WheelCollider wheel in motorWheels)
            {
                wheel.brakeTorque = brake;
            }
        }
        else
        {
            rb.drag = 0.1f;
        }

        foreach (WheelCollider wheel in motorWheels)
        {
            wheel.motorTorque = -speed * driverSeat.vertical;
            float Vel = -transform.InverseTransformDirection(rb.velocity).z;
            if ((Vel * driverSeat.vertical) < 0f)
            {
                wheel.brakeTorque = brake;
            }
            else
                wheel.brakeTorque = 0;
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
