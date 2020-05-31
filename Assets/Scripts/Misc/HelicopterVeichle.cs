﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterVeichle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.SetParent(null);
    }
}
