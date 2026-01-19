using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{

    private Chunk ownerChunk;
    private bool triggered;

    private void Awake()
    {
        ownerChunk = GetComponentInParent<Chunk>();

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) { return; }

        if (!other.CompareTag("Player")) { return; }

        triggered = true;
        ownerChunk.OnPlayerEnteredChunk();
    }
}
