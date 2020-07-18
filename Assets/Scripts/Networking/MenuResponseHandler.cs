using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuResponseHandler : MonoBehaviour
{
    public delegate void MenuHandler(int fromClient, int response);
    /// <summary>
    /// Menu handler registry
    /// </summary>
    public static Dictionary<string,MenuHandler> handlers = new Dictionary<string, MenuHandler>()
        {
            { "building_selector", MenuResponseHandler.BuildingSelected },
            { "vec_selector", MenuResponseHandler.VeichleSelected }
        };


    public static void BuildingSelected(int fromClient, int response)
    {
        Server.clients[fromClient].player.builder.selectedPart = response;
    }

    public static void VeichleSelected(int fromClient, int response)
    {
        Debug.Log("Spawning car delivery");
        EntitySpawner.instance.SpawnCar(Server.clients[fromClient].player.transform.position,response);
        Debug.Log("Spawning car delivery 2");
    }
}
