using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();

        Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
    }

    public static void SpawnCar(int _fromClient, Packet _packet)
    {
        Vector3 _pos = _packet.ReadVector3();

        EntitySpawner.instance.SpawnCar(_pos);
    }

    public static void Interact(int _fromClient, Packet _packet)
    {
        Quaternion _rot = _packet.ReadQuaternion();
        KeyCode _code = (KeyCode)_packet.ReadInt();
        Server.clients[_fromClient].player.look.rotation = _rot;
        Server.clients[_fromClient].player.Key(_code);
    }

    public static void MenuResponse(int _fromClient, Packet _packet)
    {
        string menu = _packet.ReadString();
        int response = _packet.ReadInt();

        MenuResponseHandler.handlers[menu].Invoke(_fromClient,response);
        Debug.Log("Reveived menu response");
        ServerSend.OpenMenu(_fromClient,menu,false);
    }

    public static void InventoryRequest(int _fromClient, Packet _packet)
    {
        ItemStack[] stacks = Server.clients[_fromClient].player.inventorySystem.stacks;
        int stacksLength = stacks.Length;

        ServerSend.PlayerInventory(_fromClient, stacksLength, stacks);
    }

    public static void InventoryMod(int _fromClient, Packet _packet)
    {
        int from = _packet.ReadInt();
        int to = _packet.ReadInt();
        Server.clients[_fromClient].player.inventorySystem.Transfer(from, to);
    }


}
