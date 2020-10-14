using Jint;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Interactable
{
    public string actualConsoleLog;
    private string display;

    public string inputBuffer;

    public Player user;

    public Engine javascript;

    private void Start()
    {
        javascript = new Engine();
    }

    private void Update()
    {
        inputBuffer = user.inputString;
    }

    public override void Interact(Player player = null)
    {
        user = player;
        player.inputString = "";
    }

}
