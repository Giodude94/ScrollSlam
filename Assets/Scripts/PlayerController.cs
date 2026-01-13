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
        if(state != PlayerState.Idle) { return; }

        HandleLaunchCharge();
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

        state = PlayerState.Running;

        //rb.simulated = true;
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

    void Slam()
    {
        if(state != PlayerState.Running) { return; }

        //Cancelling upward momentum befor slamming
        if (rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

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
    public void BounceOffEnemy(Vector2 contactPoint)
    {
        Debug.Log("Bounce Off Enemy is called");
        if (state != PlayerState.Running) { return; }

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
