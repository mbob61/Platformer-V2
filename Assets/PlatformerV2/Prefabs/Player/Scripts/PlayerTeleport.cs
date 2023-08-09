using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currentTeleporter;
    private PlayerV4 player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleport"))
        {
            currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == currentTeleporter)
        {
            currentTeleporter = null;
        }
    }

    public void teleport(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentTeleporter != null)
            {
                player = GetComponent<PlayerV4>();
                if (player.getDashTrailState()) player.disableDashTrail();
                if (player.getJumpTrailState()) player.disableJumpTrail();
                transform.position = currentTeleporter.GetComponent<Teleporter>().getDestination().position;

            }
        }
    }
}
