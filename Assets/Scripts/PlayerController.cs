using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float launchForce = 1100f;
    public float slamForce = 35f;

    bool canSlam = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(float timing = 1f)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.right * launchForce * timing);
        canSlam = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSlam)
            Slam();
    }

    void Slam()
    {
        rb.velocity = new Vector2(rb.velocity.x, -slamForce);
        canSlam = false;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.otherCollider.CompareTag("Ground"))
            canSlam = true;
    }
}
