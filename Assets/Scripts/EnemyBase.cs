using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyBase : MonoBehaviour
{
    [SerializeField] bool destroysOnHit = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enemy hit player");
        Debug.Log("Collision from: " + collision.gameObject.tag);
        if (!collision.gameObject.CompareTag("Player")) {
            Debug.Log("Collider is not from player."); 
            return; }

        PlayerController player = collision.collider.GetComponent<PlayerController>(); 
        
        //Player Validity Check
        if (player == null) {
            Debug.Log("Player check is null."); 
            return; }


        Vector2 contactPoint = collision.contacts[0].point;

        player.BounceOffEnemy(contactPoint);

        if (destroysOnHit) 
        {
            Destroy(gameObject);
        }


    }
}
