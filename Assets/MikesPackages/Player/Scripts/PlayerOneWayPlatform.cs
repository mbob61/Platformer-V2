using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOneWayPlatform : MonoBehaviour
{
    private GameObject currentOneWayPlatform;

    [SerializeField] private CircleCollider2D playerCollider;

    public void dropThroughOneWayPlatform(InputAction.CallbackContext context)
    {
        if (context.performed && currentOneWayPlatform != null)
        {
            StartCoroutine(drop());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator drop()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(platformCollider, playerCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(platformCollider, playerCollider, false);

    }
}
