using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : EnemyBase
{

    [SerializeField] private float moveSpeed = 5f;

    protected override void Update()
    {
        base.Update();
        Move();
    }

    void Move()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }


}