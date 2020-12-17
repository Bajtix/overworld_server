using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaCommands : MonoBehaviour
{
    public static LuaVM commandVM;




    private void Start()
    {
        Debug.Log("Start called");
        commandVM = new LuaVM(LuaVM.VMSettings.None);
        commandVM.AttachCustomAPI(typeof(CommandApi));
        
        Logger.OnLog += Logger_OnLog;
        Application.logMessageReceived += Application_logMessageReceived;
        Debug.Log("Attach logged");

        //Set commands as default (not cmd.simg but only simg)
        commandVM.ExecuteString("simg = cmd.simg");
        commandVM.ExecuteString("spawn = cmd.spawn");
    }


    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {      
        ServerSend.ConsoleMessage($"[{type}]" + condition);
    }

    private void Logger_OnLog(Channel channel, Priority priority, string message)
    {
        if(channel == Channel.Lua)
        {
            ServerSend.ConsoleMessage(message);
        }
    }

    public static void ExecCmd(int fc, string cmd)
    {
        commandVM.SetGlobal("__me", fc);
        
        commandVM.ExecuteString(cmd);
    }

    
}


[LuaApi(
    luaName = "cmd",
    description = "Command api")]
public class CommandApi : LuaAPIBase
{
    public CommandApi() : base("cmd")
    {
        
    }

    protected override void InitialiseAPITable()
    {
        m_ApiTable["simg"] = (System.Func<string, int, string>)(simg);
        m_ApiTable["spawn"] = (System.Func<string, int, string>)(spawn);
    }

    [LuaApiFunction(name = "simg",description = "Spawn image with url")]
    private string simg(string url, int player)
    {
        try
        {            
            EntitySpawner.instance.SpawnEntity("prp_pic", Server.clients[player].player.transform.position, Quaternion.identity, url);
            return "ok!";
        }
        catch
        {
            return "something fucked up!";
        }
    }

    [LuaApiFunction(name = "spawn", description = "Spawn entity")]
    private string spawn(string mdl, int player)
    {
        try
        {
            Debug.Log("spawn");
            Debug.Log(mdl);
            EntitySpawner.instance.SpawnEntity(mdl, Server.clients[player].player.transform.position, Quaternion.identity);
            Debug.Log("end spawn");
            return "ok!";
        }
        catch
        {
            return "something fucked up!";
        }
    }
}
