using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnArea
{
    public BoxCollider2D collider;

    public Vector3 GetRandomWorldPoint(Transform chunkTransform)
    {
        if (collider == null) { return Vector3.zero; }

        Bounds b = collider.bounds;
        
        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);

        //Convert World to Local space 
        return new Vector3(x,y, 0f);
    }
}
