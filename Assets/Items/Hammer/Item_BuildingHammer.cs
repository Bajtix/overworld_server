using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_BuildingHammer : GameItem
{

    public override void Selected()
    {
        //ServerSend.SendInfo(itemOwner.id, "Hammer: LMB = Build, RMB = Remove, Q = Choose Part, R = Rotate");
    }
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
        itemOwner.builder.rotation += 1;
        if (itemOwner.builder.rotation > 3)
            itemOwner.builder.rotation = 0;
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
        ServerSend.SendInfo(itemOwner.id, "");
    }
}
