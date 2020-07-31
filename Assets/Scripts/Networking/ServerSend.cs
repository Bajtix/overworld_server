using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    #region Send Methods
    /// <summary>Sends a packet to a client via TCP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    

    /// <summary>Sends a packet to a client via UDP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    /// <summary>Sends a packet to all clients via TCP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via TCP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    /// <summary>Sends a packet to all clients via UDP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }


    /// <summary>Sends a packet to all clients except one via UDP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }
    #endregion

    #region Packets
    /// <summary>Sends a welcome message to the given client.</summary>
    /// <param name="_toClient">The client to send the packet to.</param>
    /// <param name="_msg">The message to send.</param>
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(TerrainSettings.instance.seed);
            _packet.Write(TerrainSettings.instance.noiseScale);
            _packet.Write(TerrainSettings.instance.multiplier);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>Tells a client to spawn a player.</summary>
    /// <param name="_toClient">The client that should spawn the player.</param>
    /// <param name="_player">The player to spawn.</param>
    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>Sends a player's updated position to all clients.</summary>
    /// <param name="_player">The player whose position to update.</param>
    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.cspeed);
            _packet.Write((int)_player.state);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>Sends a player's updated rotation to all clients except to himself (to avoid overwriting the local player's rotation).</summary>
    /// <param name="_player">The player whose rotation to update.</param>
    public static void PlayerRotation(Player _player, bool exception = true)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);
            
            if(exception)
                SendUDPDataToAll(_player.id, _packet);
            else
                SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerHealth(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerHealth))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.health);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerRespawned(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRespawned))
        {
            _packet.Write(_player.id);

            SendTCPDataToAll(_packet);
        }
    }

    

    public static void CreateItemSpawner(int _toClient, int _spawnerId, Vector3 _spawnerPosition, bool _hasItem)
    {
        using (Packet _packet = new Packet((int)ServerPackets.createItemSpawner))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_spawnerPosition);
            _packet.Write(_hasItem);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void ItemSpawned(int _spawnerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemSpawned))
        {
            _packet.Write(_spawnerId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void ItemPickedUp(int _spawnerId, int _byPlayer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemPickedUp))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_byPlayer);

            SendTCPDataToAll(_packet);
        }
    }

    public static void SpawnEntity(Entity entity)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnEntity))
        {
            _packet.Write(entity.id);
            _packet.Write(entity.transform.position);
            _packet.Write(entity.transform.rotation);
            _packet.Write(entity.modelId);
            _packet.Write(entity.parentId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void SpawnEntity(int to,Entity entity)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnEntity))
        {
            _packet.Write(entity.id);
            _packet.Write(entity.transform.position);
            _packet.Write(entity.transform.rotation);
            _packet.Write(entity.modelId);
            _packet.Write(entity.parentId);

            SendTCPData(to,_packet);
        }
    }

    public static void KillEntity(int id)
    {
        using (Packet _packet = new Packet((int)ServerPackets.killEntity))
        {
            _packet.Write(id);
            SendTCPDataToAll(_packet);
        }
    }

    public static void EntityPosition(Entity entity)
    {
        using (Packet _packet = new Packet((int)ServerPackets.entityPosition))
        {
            _packet.Write(entity.id);
            _packet.Write(entity.transform.position);
            _packet.Write(entity.transform.rotation);
            if(entity.additionalData != null)
                _packet.Write(entity.additionalData);
            else
                _packet.Write("");
            SendUDPDataToAll(_packet);
        }
    }

    public static void ChunkMod(ChunkMod c)
    {
        using (Packet _packet = new Packet((int)ServerPackets.chunkMod))
        {
            _packet.Write((int)c.type);
            _packet.Write(c.chunk);
            _packet.Write(c.objectId);
            _packet.Write(c.modelId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void ChunkMod(ChunkMod c,int to)
    {
        using (Packet _packet = new Packet((int)ServerPackets.chunkMod))
        {
            _packet.Write((int)c.type);
            _packet.Write(c.chunk);
            _packet.Write(c.objectId);
            _packet.Write(c.modelId);

            SendTCPData(to,_packet);
        }
    }

    public static void Time(long time,float cloudDensity)
    {
        using (Packet _packet = new Packet((int)ServerPackets.time))
        {
            _packet.Write(time);
            _packet.Write(cloudDensity);
            SendUDPDataToAll(_packet);
        }
    }


    public static void PlayerInfo(int player, string toolName)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerInfo))
        {
            _packet.Write(player);
            _packet.Write(toolName);
            SendTCPDataToAll(_packet);
        }
    }

    public static void OpenMenu(int player, string menuName, bool open = true)
    {
        using (Packet _packet = new Packet((int)ServerPackets.openGUI))
        {
            _packet.Write(menuName);
            _packet.Write(open);
            SendTCPData(player,_packet);
        }
    }

    public static void PlayerInventory(int fromClient, int stacksLength, ItemStack[] stacks)
    {
        using (Packet _packet = new Packet((int)ServerPackets.inventory))
        {
            _packet.Write(stacksLength);
            for(int i = 0; i < stacksLength; i++)
            {
                if(stacks[i] == null)
                {
                    _packet.Write("none");
                    _packet.Write(0);
                }
                else
                {
                    if (stacks[i].item != null)
                        _packet.Write(stacks[i].item.name);
                    else
                        _packet.Write("none");
                    _packet.Write(stacks[i].count);
                }
                
            }
            SendTCPData(fromClient, _packet);
        }
    }

    public static void SendInfo(int player, string info)
    {
        using (Packet _packet = new Packet((int)ServerPackets.info))
        {
            _packet.Write(info);
            SendUDPData(player, _packet);
        }
    }

    public static void SendItemResponse(int player, int response)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemResponse))
        {
            _packet.Write(player);
            _packet.Write(response);
            SendTCPDataToAll(_packet);
        }
    }

    #endregion
}
