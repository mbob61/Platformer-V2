using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private PlayerV4 player;
    [SerializeField] private float bounceForce = 25.0f;
    [SerializeField] private bool upOnly = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerV4>();
            Rigidbody2D rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (player.getDashTrailState()) player.disableDashTrail();
            player.enableJumpTrail();

            player.forceEnableDoubleJump();
            rigidbody.velocity = upOnly ? Vector2.zero : new Vector2(rigidbody.velocity.x, 0.0f);
            rigidbody.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}