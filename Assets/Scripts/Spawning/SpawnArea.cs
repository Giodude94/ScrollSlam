using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] private EnemyType allowedType;
    private Collider2D col;

    public EnemyType AllowedType => allowedType;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public Vector2 GetRandomPoint()
    {
        Bounds b = col.bounds;

        return new Vector3(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y),
            0f
            );

    }
}
