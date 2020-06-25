using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDeliveryPhone : GameItem
{
    public override void MainClick()
    {
        //EnitySpawner.instance.SpawnCar(itemOwner.transform.position);
        ServerSend.OpenMenu(itemOwner.id, "vec_selector");
    }
}
