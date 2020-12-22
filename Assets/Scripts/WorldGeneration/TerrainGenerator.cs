using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider meshCollider;

    public Dictionary<int, ChunkObject> objects;

    int nid = 0;


    public float _mtnNoiseScale = 0.02f;
    public float _baseDetailScale = 0.03f;
    public float _mountainPlateScale = 0.005f;

    public float mtnNoiseScale = 0.02f;
    public float baseDetailScale = 0.03f;
    public float detailLayers = 0f;
    public float mountainPlateScale = 0.0005f;

    public float mountainMainScale = 5;

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
        SetVars();

        //Debug.Log("GENERATE: SET VERTS");
        mesh = GetComponent<MeshFilter>().mesh;
        meshCollider = GetComponent<MeshCollider>();
        Vector3[] verts = mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            var v = verts[i];
            verts[i].y = GetHeight(transform.position.x + v.x + TerrainSettings.instance.seed / 1.1233464534f, 
                transform.position.z + v.z + TerrainSettings.instance.seed / 2 / 1.165882234141f);

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
            if (Physics.Raycast(new Vector3(transform.position.x + x, 800, transform.position.z + y), new Vector3(0, -1, 0), out hit))
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
            if (Physics.Raycast(new Vector3(transform.position.x + x, 1000, transform.position.z + y), new Vector3(0, -1, 0), out hit))
            {
                GameObject o = Instantiate(TerrainSettings.instance.detailPrefabs[selected], hit.point, Quaternion.identity, transform);
                o.transform.Rotate(new Vector3(0, r.Next(0, 360), 0));
                AddFeature(o,false);
            }

            yield return new WaitForEndOfFrame();
        }
        //Debug.Log($"GENERATE: DONE (ChunkCount: {ChunkManager.instance.chunkCount})");
    }

    private void SetVars()
    {
        mtnNoiseScale = _mtnNoiseScale * TerrainSettings.instance.noiseScale;
        baseDetailScale = _baseDetailScale * TerrainSettings.instance.noiseScale;
        mountainPlateScale = _mountainPlateScale * TerrainSettings.instance.noiseScale;
    }

    

    private float GetHeight(float i, float j)
    {
        //Base mounatins
        float baseMountains = Mathf.PerlinNoise(i * mtnNoiseScale, j * mtnNoiseScale);

        float mountainPlate = Mathf.PerlinNoise(i * mountainPlateScale, j * mountainPlateScale);
        float mountainGroundPlate = Mathf.PerlinNoise(i * mountainPlateScale / 2f, j * mountainPlateScale / 2f);
        float bias = Mathf.PerlinNoise(i * mountainPlateScale / 4f, j * mountainPlateScale / 4f) * 0.2f;
        float smoothness = (Mathf.PerlinNoise(i * mountainPlateScale * 4f, j * mountainPlateScale * 4f) * 0.4f) + .6f;


        baseMountains *= mountainPlate * mountainMainScale;
        baseMountains += bias;

        float pw = Mathf.PerlinNoise(i * mountainPlateScale, j * mountainPlateScale);
        float pdetail = Mathf.Pow(baseMountains, pw * 4.25f);

        //Creates and layers detail
        float detail = 0;
        for (int k = 1; k < detailLayers + 1f; k++)
        {
            float d = Mathf.PerlinNoise(i * baseDetailScale * k, j * baseDetailScale * k);
            detail += d / k;
        }

        detail *= smoothness;

        float hght = pdetail + detail * (1f / 6f);


        return hght / 1.5f * TerrainSettings.instance.multiplier;
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
