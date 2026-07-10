using System;
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
    [SerializeField]
    [Range(1,10)]
    private int maxSlams = 3;
    [SerializeField] private float slamRefillCharge;
    [SerializeField] private float slamRefillThreshold = 1f;

    [Header("Ground Slam Penalty")]
    [SerializeField] private float groundHorizontalPenalty = 0.5f;


    [Header("Limits")]
    [SerializeField] private float maxHeight = 25f;

    [Header("API Stats")]
    int slamCount = 0;
    string sessionId;

    private Rigidbody2D rb;

    [SerializeField] private float baseBounceForce = 18f;
    [SerializeField] private float slamHorizontalVelocityBonus = 2f;
    private float horizontalMomentum;
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
        SlamRechargeCheck();
    }
    private void Launch()
    {
        hasLaunched = true;
        remainingSlams = maxSlams;

        rb.velocity = Vector2.zero;

        float rad = launchAngle * Mathf.Deg2Rad;
        Vector2 launchDir = new(Mathf.Cos(rad), Mathf.Sin(rad));

        rb.AddForce(launchDir * launchForce, ForceMode2D.Impulse);
        horizontalMomentum = rb.velocity.x;

        GameManager.Instance.StartRun();
    }
    private void Slam()
    {

        Debug.Log("Slam is being called");
        isSlamming = true;
        
        remainingSlams--;

        rb.velocity = new Vector2(horizontalMomentum, 0f);
        rb.AddForce(Vector2.down * slamForce, ForceMode2D.Impulse);

        slamCount++;
        SendSlamEvent();
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
        //If the current gamestate is not currently running the game then we return and do not execute logic.
        if (GameManager.Instance.CurrentState != GameManager.GameState.Running) { return; }

        if(!other.CompareTag("Enemy")) {  return; }

        EnemyBase enemy = other.GetComponent<EnemyBase>();

        //We are interacting with the enemy
        //One slam kills enemies, therefore we are calling the subsequent charge logic here.
        if (enemy != null)
        {
            enemy.OnHitByPlayer();
            if (maxSlams != remainingSlams) 
            {
                slamRefillCharge += enemy.slamChargeValue;
            }
        }
        
        Bounce(true,enemy.BounceBonus);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isSlamming){ return; }

        if (collision.collider.CompareTag("Ground")) 
        {
            Debug.Log("Bounced on Ground");
            Bounce(false,0f);
        }
    }
    private void Bounce(bool hitEnemy, float enemyBounceBonus)
    {
        bool sucessfullSlam = isSlamming && hitEnemy;
        
        isSlamming = false;

        if (hitEnemy)
        {
            //Rewarding successful slams with extra horizontal speed.
            if (sucessfullSlam) 
            {
                horizontalMomentum *= slamHorizontalVelocityBonus;
            }
        }
        else
        {
            //Ground slam penalty
            horizontalMomentum *= groundHorizontalPenalty;
        }

        float bounceForce = baseBounceForce + enemyBounceBonus;
        
        rb.velocity = new Vector2(horizontalMomentum,0f);
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        remainingSlams = Mathf.Min(remainingSlams, maxSlams);
    }
    private void CheckFailCondition()
    {
        if (!hasLaunched) {  return; }

        if (GameManager.Instance.CurrentState != GameManager.GameState.Running) {  return; }

        //If x and y velocity are 0 then the game is over. The player cannot bounce anymore.
        if (Mathf.Abs(rb.velocity.x) <= .0f  & Mathf.Abs(rb.velocity.y) <= .0f)
        {
            GameManager.Instance.GameOver();
        }
    }
    private void SlamRechargeCheck()
    {
        while (slamRefillCharge >= slamRefillThreshold)
        {
            slamRefillCharge -= slamRefillThreshold;
            remainingSlams++;
            //Mathf.Clamp(remainingSlams, 0, maxSlams);
        }
    }
    private void SendSlamEvent()
    {
        GameEvent e = new GameEvent
        {
            playerId = ApiClient.Instance.playerID,
            sessionId = ApiClient.Instance.sessionID, 
            eventType = "slams",
            slams = slamCount,
            x = transform.position.x,
            y = transform.position.y,
            timestamp = System.DateTime.UtcNow.ToString("o"),
         };

        string json = JsonUtility.ToJson(e);

        ApiClient.Instance.SendEvent(json);
    }
    public int GetCurrentSlam() { return remainingSlams; }
    public int GetMaxSlam() { return maxSlams; }
    public float GetSlamFillCharge() { return slamRefillCharge; }
    public float GetThresholdValue() { return slamRefillThreshold; }
}
