using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float launchForce = 50f;
    public float launchAngle = 45f;
    public float slamForce = 35f;

    [SerializeField] private float maxUpwardSpeed = 80f;
    [SerializeField] private float maxDownwardSpeed = -12f;
    [SerializeField] private float enemyBounceForce = 12f;

    bool canSlam = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(float timing = 1f)
    {
        rb.velocity = Vector2.zero;

        Vector2 dir = Quaternion.Euler(0, 0, launchAngle) * Vector2.right;
        rb.AddForce(dir * launchForce, ForceMode2D.Impulse);
        //Vector2 launchDirection = new Vector2(1f, .5f).normalized;
        //rb.AddForce(launchDirection * launchForce * timing, ForceMode2D.Impulse);
        canSlam = true;
    }

    private void FixedUpdate()
    {
        Vector2 vel = rb.velocity;
        vel.y = Mathf.Clamp(vel.y, maxDownwardSpeed, maxUpwardSpeed);
        rb.velocity = vel;
    }
    void Update()
    {
        //Slam Logic
        if (Input.GetMouseButtonDown(0) && canSlam)
            Slam();

        //Temporary logic for using spacebar to launch
        if (Input.GetKey(KeyCode.Space))
            Launch(launchForce);
    }

    void BounceOffEnemy()
    {
        // Direction from enemy to player
        Vector2 bounceDir = (transform.position).normalized;

        // Strong upward bias (important for feel)
        bounceDir.y = Mathf.Abs(bounceDir.y) + 0.5f;
        bounceDir.Normalize();

        rb.velocity = Vector2.zero;
        rb.AddForce(bounceDir * enemyBounceForce, ForceMode2D.Impulse);

        canSlam = true; // re-enable slam after bounce
    }

    void Slam()
    {
        rb.velocity = new Vector2(rb.velocity.x, -slamForce);
        canSlam = false;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.otherCollider.CompareTag("Ground"))
        {
            Debug.Log("The player has collided with the ground.");
            canSlam = true;
        }


    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("The player has collided with the enemy sprite.");
            BounceOffEnemy();
            Destroy(collider.gameObject);
        }
    }

}
