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

    public override void Alternative()
    {
        ServerSend.OpenMenu(itemOwner.id, "building_selector");
        //itemOwner.builder.selectedPart++;

        //if (itemOwner.builder.selectedPart == itemOwner.builder.parts.Length)
            //itemOwner.builder.selectedPart = 0;

    }

    public override void Tick()
    {
        itemOwner.builder.UpdatePreview();
    }

    public override void Deselected()
    {
        itemOwner.builder.preview.transform.position = new Vector3(-69,-69,-69);
    }
}
