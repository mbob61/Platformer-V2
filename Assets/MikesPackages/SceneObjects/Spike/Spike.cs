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
            PlayerV4 player = collision.gameObject.GetComponent<PlayerV4>();
            //bouncePlayer(player);
        }
    }

    private void renableInput()
    {
        player.getPlayerInput().ActivateInput();
    }

    private void bouncePlayer(PlayerV4 player)
    {
        Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();

        if (player.getDashTrailState()) player.disableDashTrail();
        player.enableJumpTrail();

        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(new Vector2(-rigidbody.velocity.x + -player.gameObject.transform.localScale.x * bounceForce.x, bounceForce.y), ForceMode2D.Impulse);

        // Add logic to take players movement away
        player.getPlayerInput().DeactivateInput();
        Invoke(nameof(renableInput), 0.5f);
    }
}
