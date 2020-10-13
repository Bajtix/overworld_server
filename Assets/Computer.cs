using Jint;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public string actualConsoleLog;
    private string display;

    public string inputBuffer;

    public Engine javascript;

    private void Start()
    {
        javascript = new Engine();
    }

    private void Update()
    {
        
    }

}
