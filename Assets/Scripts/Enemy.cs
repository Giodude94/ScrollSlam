using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : EnemyBase
{
    public enum EnemyType{
        Ground,
        Air }

    [Header("Enemy Type")]
    [SerializeField] EnemyType enemyType;

    protected override void HandlePlayerCollision(Collider2D player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller == null ) { return; }

        Vector2 contactPoint = transform.position;

        switch (enemyType)
        {
            case EnemyType.Ground:
                HandleGroundEnemy(controller, contactPoint);
                break;

            case EnemyType.Air:
                HandleAirEnemy(controller, contactPoint);
                break;
        }
    }

    void HandleGroundEnemy(PlayerController controller, Vector2 contactPoint)
    {
        //Ground enemies expect slams
        if (controller.isSlamming)
        {
            controller.BounceOffEnemy(contactPoint);
            Die();
        }
        else
        {
            //Player failed to slam onto enemy
            Die();
        }
    }

    void HandleAirEnemy(PlayerController controller, Vector2 contactPoint)
    {
        controller.BounceOffEnemy(contactPoint);
        Die();
    }


}