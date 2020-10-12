using NLua;
using System;
using UnityEngine;


public class LuaLibs
{
    public static void Hi()
    {
        Debug.Log("Hi");
    }

    public static void RegisterLuaFunctions()
    {
        Debug.Log("Register lua methods");
        
        Server.luaState.RegisterFunction("hi", typeof(LuaLibs).GetMethod("Hi"));


        Debug.Log("Registered methods");
    }
}

