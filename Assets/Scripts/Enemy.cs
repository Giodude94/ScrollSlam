using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float bounceMultiplier = 0.4f;
    public float minBounce = 5f;
    public float maxBounce = 15f;

    public void Explode()
    {
        // Optional: VFX, sound, destroy enemy
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.otherCollider.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collider.gameObject.GetComponent<Rigidbody2D>();
            float bouncePower = Mathf.Clamp(playerRb.velocity.magnitude * bounceMultiplier, minBounce, maxBounce);
            playerRb.velocity = new Vector2(playerRb.velocity.x, bouncePower);
            Explode();
        }
    }
}