using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]


public class PlayerController : MonoBehaviour
{
    [Header("Launch")]
    [SerializeField] private float launchForce = 22f;
    [SerializeField] private float launchAngle = 45f;

    [Header("Slam")]
    [SerializeField] private float slamForce = 40f;
    [SerializeField] private float bounceForce = 18f;
    [SerializeField] private int maxSlams = 3;

    [Header("Ground Slam Penalty")]
    [SerializeField] private float groundHorizontalMultiplier = 0.2f;


    [Header("Limits")]
    [SerializeField] private float maxHeight = 25f;

    private Rigidbody2D rb;

    private float slamStartHorizontalVelocity;
    private int remainingSlams;
    private bool hasLaunched;
    public bool isSlamming;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
    void Start()
    {
        remainingSlams = maxSlams;
    }

     void FixedUpdate()
    {
        ClampCeiling();
    }

    void Update()
    {
        Debug.Log(GameManager.Instance.CurrentState);
        //If the game is over then the inputs of the player should not be valid.
        if (GameManager.Instance.CurrentState == GameManager.GameState.GameOver) { return; }
        
        //Launching on left click down.
        if (!hasLaunched && Input.GetMouseButtonDown(0))
        {
            Launch();
            return;
        }
        //If player has launched and there are remaining slams && left click is down.
        if (hasLaunched && !isSlamming && remainingSlams > 0 && Input.GetMouseButtonDown(0))
        {
            Slam();
        }

        CheckFailCondition();
    }
    private void Launch()
    {
        hasLaunched = true;
        remainingSlams = maxSlams;

        rb.velocity = Vector2.zero;

        float rad = launchAngle * Mathf.Deg2Rad;
        Vector2 launchDir = new(Mathf.Cos(rad), Mathf.Sin(rad));

        rb.AddForce(launchDir * launchForce, ForceMode2D.Impulse);

        GameManager.Instance.StartRun();
    }

    private void Slam()
    {
        Debug.Log("Slam is being called");
        isSlamming = true;
        remainingSlams--;

        slamStartHorizontalVelocity = rb.velocity.x;

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.down * slamForce, ForceMode2D.Impulse);
    }

    private void ClampCeiling()
    {
        if (transform.position.y <= maxHeight) { return; }

        transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);

        if (rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //This logic only makes the player bounce if they are slamming. Uncommented to make logic occur regardless of slamming.
        //if (!isSlamming) { return; }

        if (other.CompareTag("Enemy"))
        {
           EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.OnHitByPlayer();
            }
            Bounce(true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isSlamming){ return; }

        if (collision.collider.CompareTag("Ground")) 
        {
            Debug.Log("Bounced on Ground");
            Bounce(false);
        }
    }

    private void Bounce(bool hitEnemy)
    {
        isSlamming = false;

        float currentX = slamStartHorizontalVelocity;

        if (!hitEnemy)
        {
            currentX *= groundHorizontalMultiplier;
        }

        rb.velocity = new Vector2(currentX,0f);
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        remainingSlams = Mathf.Min(remainingSlams + 1, maxSlams);
    }

    private void CheckFailCondition()
    {
        if (!hasLaunched) {  return; }

        if (GameManager.Instance.CurrentState != GameManager.GameState.Running) {  return; }

        if (Mathf.Abs(rb.velocity.x) <= .01f)
        {
            GameManager.Instance.GameOver();
        }
    }
}
