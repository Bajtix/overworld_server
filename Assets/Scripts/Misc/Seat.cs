using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [System.NonSerialized]
    public Player controller;

    public Transform pos;
    public float vertical;
    public float horizontal;

    public void SetInputs(float v, float h)
    {
        vertical = v;
        horizontal = h;
    }

    public void TakeASeat(Player player)
    {
        player.controller.enabled = false; //makes sure the CC on the player doesn't interrupt
        controller = player;
        Debug.Log("Entered seat. Player: " + controller.id);
    }

    public void LeaveSeat()
    {
        if (controller != null)
        {
            controller.transform.position += new Vector3(0, 10, 0);
            controller.controller.enabled = true; //leaving the seat enables controller
            Debug.Log(controller.controller.enabled);
            Debug.Log("Seat left");
            controller = null;
        }
        else
        {
            Debug.Log("Leaving seat failed controller already null");
        }
    }

    private void Update()
    {
        if (controller != null)
        {
            controller.transform.position = pos.position;
            //controller.transform.rotation = pos.rotation;
        }
    }
}
