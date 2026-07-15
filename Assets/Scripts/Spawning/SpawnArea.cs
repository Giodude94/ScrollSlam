using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] private SpawnAreaType areaType;
    private Collider2D col;

    public SpawnAreaType AreaType => areaType;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public Vector2 GetRandomYPos()
    {
        Bounds b = col.bounds;

        return new Vector2(
            0f,
            Random.Range(b.min.y, b.max.y)
            );

    }
}
