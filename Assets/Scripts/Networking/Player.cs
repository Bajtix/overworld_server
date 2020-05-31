using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController controller;
    public Transform shootOrigin;
    public float gravity = -9.81f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;
    public float throwForce = 600f;
    public float health;
    public float maxHealth = 100f;
    public int itemAmount = 0;
    public int maxItemAmount = 3;

    private bool[] inputs;
    private float yVelocity = 0;

    public Seat seatIn;
    private float interactTimeout;

    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        inputs = new bool[5];
    }

    /// <summary>Processes player input and moves the player.</summary>
    public void FixedUpdate()
    {
        if (interactTimeout > 0) interactTimeout -= Time.deltaTime;
        ChunkManagement();

        if (health <= 0f)
        {
            return;
        }

        Vector2 _inputDirection = Vector2.zero;
        if (inputs[0])
        {
            _inputDirection.y += 1;
        }
        if (inputs[1])
        {
            _inputDirection.y -= 1;
        }
        if (inputs[2])
        {
            _inputDirection.x -= 1;
        }
        if (inputs[3])
        {
            _inputDirection.x += 1;
        }

        if (seatIn == null)
        {
            if(inputs[5] && interactTimeout <= 0)
            {
                interactTimeout = 2;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
                {
                    if(hit.collider!=null)
                    if(hit.collider.GetComponent<Seat>() != null)
                    {
                        seatIn = hit.collider.GetComponent<Seat>();
                        seatIn.TakeASeat(this);
                    }
                }
            }


            Move(_inputDirection);
        }
        else
        {
            seatIn.SetInputs(_inputDirection.y, _inputDirection.x);
            if (inputs[5] && interactTimeout<=0)
            {
                interactTimeout = 2;
                seatIn.LeaveSeat();
                seatIn = null;
            }
            
        }

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    private void ChunkManagement()
    {
        var chunkPos = ChunkManager.ChunkAt(transform.position.x, transform.position.z);


        for (int i = -TerrainSettings.instance.renderDistance; i < TerrainSettings.instance.renderDistance; i++)
        {
            for (int j = -TerrainSettings.instance.renderDistance; j < TerrainSettings.instance.renderDistance; j++)
            {
                ChunkManager.instance.AddChunk(chunkPos.x + i, chunkPos.y + j);
            }
        }


        for (int i = -TerrainSettings.instance.renderDistance; i < TerrainSettings.instance.renderDistance; i++)
        {
            ChunkManager.instance.RemoveChunk(chunkPos.x + i, chunkPos.y + TerrainSettings.instance.renderDistance + 1);
        }

        for (int i = -TerrainSettings.instance.renderDistance; i < TerrainSettings.instance.renderDistance; i++)
        {
            ChunkManager.instance.RemoveChunk(chunkPos.x + i, chunkPos.y - TerrainSettings.instance.renderDistance - 1);
        }

        for (int i = -TerrainSettings.instance.renderDistance - 1; i < TerrainSettings.instance.renderDistance + 1; i++)
        {
            ChunkManager.instance.RemoveChunk(chunkPos.x + -TerrainSettings.instance.renderDistance - 1, chunkPos.y + i);
        }

        for (int i = -TerrainSettings.instance.renderDistance - 1; i < TerrainSettings.instance.renderDistance + 1; i++)
        {
            ChunkManager.instance.RemoveChunk(chunkPos.x + TerrainSettings.instance.renderDistance + 1, chunkPos.y + i);
        }
    }

    /// <summary>Calculates the player's desired movement direction and moves him.</summary>
    /// <param name="_inputDirection"></param>
    private void Move(Vector2 _inputDirection)
    {
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

        if (controller.isGrounded)
        {
            yVelocity = 0f;
            if (inputs[4])
            {
                yVelocity = jumpSpeed;
            }
        }
        yVelocity += gravity;

        _moveDirection.y = yVelocity;
        controller.Move(_moveDirection);

        
    }

    /// <summary>Updates the player input with newly received input.</summary>
    /// <param name="_inputs">The new key inputs.</param>
    /// <param name="_rotation">The new rotation.</param>
    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }


}
