﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class _GameUtilityToolset
{
    public static int FirstFree(Dictionary<int, EntityManager> dict, int minumum)
    {
        int min = dict.Count == 0
            ? minumum                       //use passed minimum if needed
            : dict.Keys.Min();              //use real minimum
        int max = dict.Count > 1
            ? dict.Keys.Max() + 2           //two steps away from maximum, avoids exceptions
            : min + 2;                      //emulate data presence
        return Enumerable.Range(min, max).Except(dict.Keys).First();
    }

    public static LayerMask GetPhysicsLayerMask(int currentLayer)
    {
        int finalMask = 0;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(currentLayer, i)) finalMask = finalMask | (1 << i);
        }
        return finalMask;
    }
}
