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
    public GameObject projectilePrefab;

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

    public Player InstantiatePlayer()
    {
        RaycastHit hit;
        Physics.Raycast(new Vector3(1500f, 800f, 1500f), Vector3.down, out hit);
        return Instantiate(playerPrefab, hit.point + new Vector3(0,10,0) /*new Vector3(1600,300,1600)*/, Quaternion.identity).GetComponent<Player>();
    }

    public Entity SpawnNewEntity(Vector3 position,Quaternion rotation, string model,int parentId)
    {
        if(parentId != -9999)
            return Instantiate(entityPrefabs[model], position, rotation,Server.entities[parentId].entity.transform).GetComponent<Entity>();
        else
            return Instantiate(entityPrefabs[model], position, rotation).GetComponent<Entity>();
    }

}
