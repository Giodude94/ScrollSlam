using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{

    Chunk chunk;

    private void Awake()
    {
        chunk = GetComponentInParent<Chunk>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) {  return; }

        chunk.TriggerSpawn();
    }
}
