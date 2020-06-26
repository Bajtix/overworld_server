﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public static ChunkManager instance;

    public GameObject terrainPrefab;
    public int chunkCount;
    public GameObject[,] chunks;

    private void Awake()
    {
        if (instance != this)
        {
            if (instance != null)
                Destroy(instance);
            instance = this;
        }

        chunks = new GameObject[6900, 6900];

        for (int i = 0; i < 6900; i++)
        {
            for (int j = 0; j < 6900; j++)
            {
                chunks[i, j] = null;
            }
        }
    }


    private void Start()
    {

        AddChunk(ChunkAt(1500, 1500).x, ChunkAt(1500, 1500).y);
    }
    public struct V2Int
    {
        public int x;
        public int y;

        public V2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2 GetVector2()
        {
            return new Vector2(x, y);
        }
    }

    public static V2Int ChunkAt(float x, float y)
    {
        return new V2Int(Mathf.FloorToInt(x / 40), Mathf.FloorToInt(y / 40));
    }

    public void AddChunk(int x, int y)
    {
        if (x < 0 || x > chunks.Length || y < 0 || y > chunks.Length)
            return;
        if (chunks[x, y] == null)
        {
            chunkCount++;
            chunks[x, y] = Instantiate(terrainPrefab, new Vector3(x * 40, 0, y * 40), Quaternion.identity);
        }
    }

    public void RemoveChunk(int x, int y)
    {
        return;
        if (x < 0 || x > chunks.Length || y < 0 || y > chunks.Length)
            return;
        if (chunks[x, y] != null)
        {
            chunkCount--;
            Destroy(chunks[x, y]);
            chunks[x, y] = null;
        }
    }

}
