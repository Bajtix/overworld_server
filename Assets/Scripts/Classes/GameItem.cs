﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    public Player itemOwner;
    public Item myItem;

    private void OnDestroy()
    {
        Deselected();
    }

    private void Start()
    {
        Selected();
    }

    public virtual void Tick()
    {

    }

    public virtual void MainClick()
    {

    }

    public virtual void SecondaryClick()
    {

    }

    public virtual void Reload()
    {

    }

    public virtual void Alternative()
    {

    }

    public virtual void Selected()
    {

    }

    public virtual void Deselected()
    {

    }
}
