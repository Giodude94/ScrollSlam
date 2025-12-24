using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{

    [Header("Chunk Settings")]
    public Chunk chunkPrefab;
    public int initialChunks = 3;
    private float chunkLength;

    [Header("Player Reference")]
    public Transform player;

    [Header("Cleanup")]
    public int maxChunksAlive = 6;

    int currentChunkIndex = 1;
    float nextSpawnX = 0f;

    readonly private Queue<Chunk> spawnedChunks= new Queue<Chunk>();

    void Start()
    {
        chunkLength = chunkPrefab.length;
        Chunk startingChunk = FindObjectOfType<Chunk>();

        if (startingChunk == null) { return; }

        startingChunk.chunkIndex = 0;
        spawnedChunks.Enqueue(startingChunk);

        nextSpawnX = startingChunk.transform.position.x + chunkLength;

        //chunkLength = chunkPrefab.length;
        //SpawnInitialChunks();
    }

    void Update()
    {
        if (player.position.x > nextSpawnX - (chunkLength * 2))
        {
            SpawnChunk();
        }
    }
    void SpawnInitialChunks()
    {
        for (int i = 0; i < initialChunks; i++)
        {
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        //Debug.Log("Next Spawnx value: " + nextSpawnX);
        Vector3 position = new Vector3(nextSpawnX, 0f, 0f);

        Chunk chunk = Instantiate(chunkPrefab, position, Quaternion.identity);

        chunk.chunkIndex = currentChunkIndex;
        currentChunkIndex++;

        spawnedChunks.Enqueue(chunk);

        nextSpawnX += chunkLength;

        CleanupOldChunks();

    }

    void CleanupOldChunks()
    {
        while (spawnedChunks.Count > maxChunksAlive)
        {
            Chunk oldChunk = spawnedChunks.Dequeue();
            Destroy(oldChunk.gameObject);
        }
    }
}