using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Camera : GameItem
{
    public override void MainClick()
    {
        ServerSend.SendItemResponse(itemOwner.id, 0);
    }
}
