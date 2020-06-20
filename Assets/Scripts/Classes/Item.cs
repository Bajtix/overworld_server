using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Holdable Item",menuName = "Game/Item/Holdable")]
public class Item : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public string description;
    public string itemTag;
    
    public int maxStackCount;
    public GameObject model;
    public float damage;
    public float durability; 
}
