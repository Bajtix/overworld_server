using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_BuildingHammer : GameItem
{
    public override void MainClick()
    {
        itemOwner.builder.BuildButton();
    }

    public override void SecondaryClick()
    {
        itemOwner.builder.DestroyButton();
    }

    public override void Reload()
    {
        itemOwner.builder.rot *= Quaternion.Euler(0,0,90);
    }
}
