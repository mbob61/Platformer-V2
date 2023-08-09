using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private Vector2 bounceForce;
    private PlayerV4 player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerV4>();
            Rigidbody2D rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            player.setDash();

            if (player.getDashTrailState()) player.disableDashTrail();
            player.enableJumpTrail();

            float f = (-rigidbody.velocity.x) + (-collision.gameObject.transform.localScale.x * bounceForce.x);

            rigidbody.velocity = Vector2.zero;
            //rigidbody.AddForce(new Vector2(f, bounceForce.y), ForceMode2D.Impulse);
            rigidbody.AddForce(new Vector2(-collision.gameObject.transform.localScale.x * bounceForce.x, bounceForce.y), ForceMode2D.Impulse);

            Invoke(nameof(cancel), 0.2f);
        }
    }

    private void cancel()
    {
        player.cancelDash();
    }
}
