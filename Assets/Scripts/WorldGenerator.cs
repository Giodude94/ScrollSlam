using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject chunkPrefab;
    public Transform player;
    public int initialChunks = 5;
    public float spawnBuffer = 20f;

    private Queue<GameObject> chunks = new Queue<GameObject>();
    private float nextSpawnX = 0f;

    void Start()
    {
        for (int i = 0; i < initialChunks; i++)
            SpawnChunk();
    }

    void Update()
    {
        if (player.position.x > nextSpawnX - spawnBuffer)
            SpawnChunk();
    }

    void SpawnChunk()
    {
        GameObject chunk = Instantiate(chunkPrefab);
        chunk.transform.position = new Vector3(nextSpawnX, 0, 0);
        chunks.Enqueue(chunk);
        nextSpawnX += chunk.GetComponent<Chunk>().length;

        //Remove oldest chunk
        if (chunks.Count > initialChunks)
            Destroy(chunks.Dequeue());
    }
}