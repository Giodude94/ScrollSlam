using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 4f;

    protected bool isAlive = true;

    protected virtual void Update()
    {
        if (isAlive) { return; }

        Move();
    }

    protected virtual void Move()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAlive) { return; }

        if (!other.CompareTag("Player")) { return; }

        HandlePlayerCollision(other);
    }

    protected abstract void HandlePlayerCollision(Collider2D player);

    protected virtual void Die()
    {
        isAlive = false;
        Destroy(gameObject);
    }
}
