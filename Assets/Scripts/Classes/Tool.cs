using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Tool", menuName = "Game/Item/Tool")]
public class Tool : Holdable
{
    public float damage;
    public float usage;
}
