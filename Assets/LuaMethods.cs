using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    public class LuaMethods
    {
        public static void print(string s)
        {
            Debug.Log(s);
        }

        public static void Main(string[] args)
        {
            print("Hello World!");
        }
    }
}
