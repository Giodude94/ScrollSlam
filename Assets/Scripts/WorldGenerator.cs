using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Chunk chunkPrefab;


    [Header("Chunk Settings")]
    //public Chunk chunkPrefab;
    //public int initialChunks = 3;
    //private float chunkLength;
    [SerializeField] private int chunksAhead = 4;
    [SerializeField] private int chunksBehind = 2;
    [SerializeField] public float chunkWidth = 19.2f;

    private Dictionary<int, Chunk> activeChunks = new();
    private int currentPlayerChunkIndex;

    void Start()
    {
        currentPlayerChunkIndex = GetChunkIndex(player.position.x);
        UpdateChunks(force: true);
    }

    void Update()
    {
        int newChunkIndex = GetChunkIndex(player.position.x);

        if (newChunkIndex != currentPlayerChunkIndex)
        {
            currentPlayerChunkIndex = newChunkIndex;
            UpdateChunks(force: false);
        }
    }
    void UpdateChunks(bool force)
    {
        int minIndex = currentPlayerChunkIndex - chunksBehind;
        int maxIndex = currentPlayerChunkIndex + chunksAhead;

        for (int i = minIndex; i <= maxIndex; i++)
        {
            if (!activeChunks.ContainsKey(i))
            {
                SpawnChunk(i);
            }
        }

        List<int> toRemove = new();
        foreach (var kvp in activeChunks)
        {
            if (kvp.Key < minIndex || kvp.Key > maxIndex)
            {
                Destroy(kvp.Value.gameObject);
                toRemove.Add(kvp.Key);
            }
        }

        foreach (int index in toRemove)
        {
            activeChunks.Remove(index);
        }
    }

    void SpawnChunk(int index)
    {
        Vector3 position = new (index * chunkWidth, 0f, 0f);
        Chunk chunk = Instantiate(chunkPrefab, position, Quaternion.identity, transform);

        chunk.Initialize(index);
        activeChunks.Add(index, chunk);

    }

    private int GetChunkIndex(float x)
    {
        return Mathf.FloorToInt(x / chunkWidth);
    }
}