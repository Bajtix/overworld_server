using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public float time;
    public float clouds;

    private void Awake()
    {
        if (instance != this) Destroy(instance);
        instance = this;
    }


    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime/6;
        if (time > 24) time = 0;
        ServerSend.Time(time, 0.4f);
    }
}
