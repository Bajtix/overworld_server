using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider meshCollider;

    public Dictionary<int, ChunkObject> objects;

    int nid = 0;


    private void Start()
    {
        objects = new Dictionary<int, ChunkObject>();
        Generate();
    }

    public void Generate()
    {
        StartCoroutine("GenerateCoroutine");
    }

    private IEnumerator GenerateCoroutine()
    {
        //Debug.Log("GENERATE: SET VERTS");
        mesh = GetComponent<MeshFilter>().mesh;
        meshCollider = GetComponent<MeshCollider>();
        Vector3[] verts = mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            var v = verts[i];
            verts[i].y = GetPass(v.x, v.z, TerrainSettings.instance.noiseScale / 70, TerrainSettings.instance.multiplier * 20)
                + GetPass(v.x, v.z, TerrainSettings.instance.noiseScale / 20, TerrainSettings.instance.multiplier * 10)
                + GetPass(v.x, v.z, TerrainSettings.instance.noiseScale, TerrainSettings.instance.multiplier)
                + GetPass(v.x, v.z, TerrainSettings.instance.noiseScale * 4, TerrainSettings.instance.multiplier / 8);
            if (i % 400 == 0)
                yield return new WaitForEndOfFrame();
        }
        //Debug.Log("GENERATE: SET VERTS OK");
        mesh.vertices = verts;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = mesh;

        StartCoroutine("SpawnTrees");
        StartCoroutine("SpawnDetails");
    }

    public IEnumerator SpawnTrees()
    {
        //Debug.Log("GENERATE: SPAWN TREES");


        System.Random r = new System.Random(Mathf.RoundToInt(TerrainSettings.instance.seed + transform.position.x + transform.position.z));

        float densityForChunk = Mathf.Pow(Mathf.PerlinNoise(transform.position.x * 0.00523f, transform.position.z * 0.00523f), 1.69f) * 25;

        for (int i = 0; i < densityForChunk; i++)
        {
            float x = r.Next(0, 40);
            float y = r.Next(0, 40);

            int selected = r.Next(0, TerrainSettings.instance.treePrefabs.Length);

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x + x, 200, transform.position.z + y), new Vector3(0, -1, 0), out hit))
            {
                GameObject o = Instantiate(TerrainSettings.instance.treePrefabs[selected], hit.point, Quaternion.identity, transform);
                o.transform.Rotate(new Vector3(0, r.Next(0, 360), 0));
                AddFeature(o,false);
            }
            
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log($"GENERATE: DONE (ChunkCount: {ChunkManager.instance.chunkCount})");
    }

    public IEnumerator SpawnDetails()
    {
       // Debug.Log("GENERATE: SPAWN DETAILS");


        System.Random r = new System.Random(Mathf.RoundToInt(TerrainSettings.instance.seed + transform.position.x + transform.position.z) / 3);

        float densityForChunk = r.Next(0, 3);

        for (int i = 0; i < densityForChunk; i++)
        {
            float x = r.Next(0, 40);
            float y = r.Next(0, 40);

            int selected = r.Next(0, TerrainSettings.instance.detailPrefabs.Length);

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x + x, 200, transform.position.z + y), new Vector3(0, -1, 0), out hit))
            {
                GameObject o = Instantiate(TerrainSettings.instance.detailPrefabs[selected], hit.point, Quaternion.identity, transform);
                o.transform.Rotate(new Vector3(0, r.Next(0, 360), 0));
                AddFeature(o,false);
            }

            yield return new WaitForEndOfFrame();
        }
        //Debug.Log($"GENERATE: DONE (ChunkCount: {ChunkManager.instance.chunkCount})");
    }

    float GetPass(float x, float z, float scale, float mult)
    {
        return GetPass(x, z, scale) * mult;
    }

    float GetPass(float x, float z, float scale)
    {
        return Mathf.PerlinNoise((transform.position.x + x + TerrainSettings.instance.seed / 1.1233464534f) * scale, (transform.position.z + z + TerrainSettings.instance.seed / 2 / 1.165882234141f) * scale);
    }

    public void AddFeature(GameObject go, bool buffer = true)
    {
        ChunkObject c = go.GetComponent<ChunkObject>(); //to get it to work, assign every ChunkObject prefab the script and add an id.
        c.myId = objects.Count;
        c.chunk = this;
        objects.Add(nid, c);
        nid++;
        if (buffer)
        {
            ChunkMod result = new ChunkMod(ChunkMod.ChunkModType.Add, go.transform.position, c.myId, c.model);
            Server.bufferedChunkmods.Add(result);
            ServerSend.ChunkMod(result);
        }
    }

    public void RemoveFeature(int id, bool buffer = true)
    {
        
        Destroy(objects[id].gameObject);
        objects.Remove(id);

        if (buffer)
        {
            ChunkMod result = new ChunkMod(ChunkMod.ChunkModType.Remove, transform.position, id, -1);
            Server.bufferedChunkmods.Add(result);
            ServerSend.ChunkMod(result);
        }
    }
}
