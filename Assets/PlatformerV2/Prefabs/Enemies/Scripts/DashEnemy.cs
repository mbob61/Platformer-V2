using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : GenericEnemy
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerV4 player = collision.gameObject.GetComponent<PlayerV4>();
            if (player.getDashState())
            {
                health--;
                if (health <= -0)
                {
                    Rigidbody2D rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

                    if (player.getDashTrailState()) player.disableDashTrail();
                    player.enableJumpTrail();

                    player.forceEnableDoubleJump();
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.0f);
                    rigidbody.AddForce(Vector2.up * bounceForceForPlayer, ForceMode2D.Impulse);
                }
            }
        }
    }
}
