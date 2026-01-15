using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]


public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Running
    }

    [Header("State")]
    [SerializeField] PlayerState state = PlayerState.Idle;

    [Header("Forward Motion")]
    [SerializeField] float baseForwardSpeed = 8f;
    [SerializeField] float airDrag = .05f;

    [Header("Launch Charge")]
    [SerializeField] float minLaunchForce = 8f;
    [SerializeField] float maxLaunchForce = 30f;
    [SerializeField] float chargeTimeToMax = 1.5f;
    [SerializeField] float launchAngleDegrees = 45f;

    float currentCharge;
    bool isCharging;

    [Header("Slam")]
    [SerializeField] float slamForce = 25f;

    [Header("Enemy Bounce")]
    [SerializeField] float enemyBounceForce = 18f;
    [SerializeField, Range(0f, 1f)] float upwardBias = 0.75f;


    [Header("Ceiling Clamp")]
    [SerializeField] float ceilingBounceDampening = .25f;

    public Rigidbody2D rb;

    [Header("Slam Charges")]
    [SerializeField] int maxSlams = 7;

    [Header("Ground Bounce")]
    [SerializeField] float groundBounceMultiplier = 0.75f;
    [SerializeField] float minGroundBounceForce = 8f;

    //int currentSlams;

    int currentSlams = 7;
    public bool isSlamming;
    bool isGrounded;

    //[SerializeField] private float maxUpwardSpeed = 80f;
    //[SerializeField] private float maxDownwardSpeed = -12f;
    //[SerializeField] float maxHeight = 25f;
    //[SerializeField] float verticalDampening = .35f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Start()
    {
        //rb.simulated = false;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Update()
    {
        if(state == PlayerState.Idle) 
        {
            HandleLaunchCharge();
            return; 
        }

        HandleSlamInput();
    }

    void HandleSlamInput()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            TrySlam();
        }
    }
    private void FixedUpdate()
    {
        if (state != PlayerState.Running) { return; }

        MaintainForwardMotion();
        ApplyAirDrag();
    }

    void HandleLaunchCharge()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            currentCharge = minLaunchForce;
        }

        if (isCharging && Input.GetMouseButtonDown(0))
        {
            currentCharge += (maxLaunchForce - minLaunchForce) * (Time.deltaTime / chargeTimeToMax);
            
            currentCharge = Mathf.Clamp(currentCharge, minLaunchForce, maxLaunchForce);
        }

        if (isCharging && Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            StartRunWithForce(currentCharge);
        }
    }

    void StartRunWithForce(float force)
    {
        if (state != PlayerState.Idle) { return; }

        currentSlams = maxSlams;

        state = PlayerState.Running;

        rb.constraints = RigidbodyConstraints2D.None;
        rb.gravityScale = 1f;
        rb.velocity = Vector2.zero;

        float angleRad = launchAngleDegrees * Mathf.Deg2Rad;
        Vector2 launchDir = new Vector2( Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;

        Physics2D.SyncTransforms();
        //Apply impulse
        rb.AddForce(launchDir * force, ForceMode2D.Impulse);

        rb.velocity = new Vector2(baseForwardSpeed, rb.velocity.y);
    }

    void MaintainForwardMotion()
    {
        //Preserving vertical velocity
        rb.velocity = new Vector2(baseForwardSpeed, rb.velocity.y);
    }

    void ApplyAirDrag()
    {
        if (rb.velocity.y != 0f)
        {
            rb.velocity = new Vector2 ( rb.velocity.x, rb.velocity.y * (1f - airDrag * Time.fixedDeltaTime));
        }
    }

    void TrySlam()
    {
        if (currentSlams <= 0) { return; }

        if(isGrounded) { return; }

        Debug.Log("Slam activating");

        currentSlams--;
        isSlamming = true;

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.down * slamForce, ForceMode2D.Impulse);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ceiling")){ return; }

        if (rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y * ceilingBounceDampening);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;

            if (isSlamming)
            {
                HandleGroundBounce(collision);
            }
        }
        HandleGroundBounce(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void HandleGroundBounce(Collision2D collision)
    {
        isSlamming = false;

        float impactSpeed = Mathf.Abs(rb.velocity.y);
        
        float bounceForce = Mathf.Max(impactSpeed * groundBounceMultiplier, minGroundBounceForce);

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
    } 
    public void BounceOffEnemy(Vector2 contactPoint)
    {
        
        if (state != PlayerState.Running) { return; }

        currentSlams = Mathf.Min(currentSlams + 1, maxSlams);

        //Cancelling downward velocity before bounce
        if (rb.velocity.y < 0f) 
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

        Vector2 awayFromEnemy = (Vector2)(transform.position - (Vector3)contactPoint).normalized;

        Vector2 bounceDir = (Vector2.up * upwardBias + awayFromEnemy * (1f - upwardBias)).normalized;

        rb.AddForce(bounceDir * enemyBounceForce, ForceMode2D.Impulse);
    }
}
