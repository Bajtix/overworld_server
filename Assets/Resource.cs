using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public float hp;
    [System.NonSerialized]
    public float spawnHp;
    public Item drop;

    public virtual void Start()
    {
        spawnHp = hp;
    }

    public virtual void Hit(Item heldItem)
    {
        hp -= heldItem.damage;
    }

    public virtual void Interact(Item heldItem)
    {

    }
}
