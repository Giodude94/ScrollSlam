using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected bool isAlive = true;

    protected virtual void Update()
    {
        if (isAlive) { return; }
    }

    public virtual void OnHitByPlayer()
    {
        if(!isAlive) { return; }
        
        Die();
    }
    protected virtual void Die()
    {
        isAlive = false;
        Destroy(gameObject);
    }
}
