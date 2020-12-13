using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [System.NonSerialized]
    public int id;
    [System.NonSerialized]
    public string modelId;
    [System.NonSerialized]
    public int parentId = -1;
    [System.NonSerialized]
    public string additionalData;
    public object additionalDataObject;
    public bool updatePos = true;
    public uint updateTick = 1;

    public bool checkPlayerNearby = true;
    public float updateDistance = 100f;
    
    private int tick = 0;

    /// <summary>
    /// Initializes entity with data
    /// </summary>
    /// <param name="id">The entities id, should be the same as server's</param>
    /// <param name="modelId">The spawned entity. </param>
    /// <param name="parentId">Parent entity</param>
    public void Initialize(int id,string modelId, int parentId)
    {
        this.id = id;
        this.modelId = modelId;
        this.parentId = parentId;
    }


    private bool IsPlayerNearby()
    {
        bool i = false;
        foreach(var p in Server.clients.Values)
        {
            if(p.player != null)
            if (Vector3.Distance(transform.position, p.player.transform.position) < updateDistance)
                i = true;
        }

        return i;
    }

    private void FixedUpdate()
    {
        bool playerNearby = true;

        if (checkPlayerNearby)
            playerNearby = IsPlayerNearby();

        if (updatePos && playerNearby)
        {
            if (tick >= updateTick)
            {
                SendEntityData();
                tick = 0; // sends updates every few ticks
            }
            tick++;
        }
    }

    public void SendEntityData()
    {
        ServerSend.EntityPosition(this);
    }
}
