using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Type")]
    public EnemyType enemyType;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Bounce Settings")]
    public float bounceMultiplier = 0.4f;
    public float minBounce = 5f;
    public float maxBounce = 15f;

    [Header("Air Enemy Settings")]
    public float airVerticalBoost = 6f;
    public float airWaveAmplitude = .5f;
    public float airWaveFrequency = 2f;

    float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        //Moving from right of screen to the left
        Vector3 pos = transform.position;
        pos.x -= moveSpeed * Time.deltaTime;

        //Behavior for air enemies
        if (enemyType == EnemyType.Air)
        {
            pos.y = startY + Mathf.Sin(Time.time * airWaveFrequency) * airWaveAmplitude;
        }

        transform.position = pos;

    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        //If collider interacts with any other object we do not execute the following logic.
        if (!collider.gameObject.CompareTag("Player")) return;

        Rigidbody2D playerRb = collider.rigidbody;

        if (!playerRb) return;

        ApplyBounce(playerRb);
        Explode();
    }

    protected virtual void ApplyBounce(Rigidbody2D playerRb)
    {
        float baseBounce = Mathf.Clamp(playerRb.velocity.magnitude * bounceMultiplier, minBounce, maxBounce);

        if (enemyType == EnemyType.Ground)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, baseBounce);
        }
        else
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, baseBounce + airVerticalBoost);
        }
    }

    public EnemyType getEnemyType()
    {
        return enemyType;
    }

    public void Explode()
    {
        // Optional: VFX, sound, destroy enemy
        Debug.Log("Destroying Game Object.");
        Destroy(gameObject);
    }
}