using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public enum EnemyCategory
    {
        Basic,
        Special,
        Heavy,
        Boss
    }
    
    protected bool isAlive = true;
    public float slamChargeValue = .25f;
    public EnemyCategory category;
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
