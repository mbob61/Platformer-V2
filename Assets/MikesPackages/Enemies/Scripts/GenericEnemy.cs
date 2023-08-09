using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : MonoBehaviour
{
    [SerializeField] protected int health = 1;
    [SerializeField] protected BoxCollider2D damageCollider;
    [SerializeField] protected float bounceForceForPlayer = 10.0f;

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (health > 0)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
