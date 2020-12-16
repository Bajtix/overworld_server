using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary; //thank you, bro! It would be so much harder without this lib.

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;
    [System.Serializable]
    public class _EDIC : SerializableDictionaryBase<string, GameObject> { }
    public _EDIC entityPrefabs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(50, 26950);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }
    /// <summary>
    /// Spawns new player into the worlds
    /// </summary>
    /// <returns>The spawned player</returns>
    public Player InstantiatePlayer()
    {
        RaycastHit hit;
        Physics.Raycast(new Vector3(1500f, 800f, 1500f), Vector3.down, out hit);
        return Instantiate(playerPrefab, hit.point + new Vector3(0,10,0) /*new Vector3(1600,300,1600)*/, Quaternion.identity).GetComponent<Player>();
    }
    /// <summary>
    /// Spawns the entity
    /// </summary>
    /// <param name="position">Entity Position</param>
    /// <param name="rotation">Entity Rotation</param>
    /// <param name="model">Entity Prefab (From entityPrefabs list)</param>
    /// <param name="parentId">Parent (-9999 if none)</param>
    /// <returns>The spawned Entity</returns>
    public Entity SpawnNewEntity(Vector3 position,Quaternion rotation, string model,int parentId, object data = null)
    {
        Entity sp;
        if(parentId != -9999)
            sp = Instantiate(entityPrefabs[model], position, rotation,Server.entities[parentId].entity.transform).GetComponent<Entity>();
        else
            sp = Instantiate(entityPrefabs[model], position, rotation).GetComponent<Entity>();
        sp.additionalDataObject = data;
        return sp;
    }
    /// <summary>
    /// Sends chunkmods to the selected player
    /// </summary>
    /// <param name="toPlayer">Player to send them to</param>
    public void SendChunkMods(int toPlayer)
    {
        StartCoroutine("Cmods",toPlayer);
    }
    private IEnumerator Cmods(int id)
    {
        Debug.Log("Sending chunkmods");
        yield return new WaitForSeconds(4);
        foreach (ChunkMod c in Server.bufferedChunkmods)
        {
            ServerSend.ChunkMod(c, id);
            yield return new WaitForEndOfFrame();
        }
    }

}
