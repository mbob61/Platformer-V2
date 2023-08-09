using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private float fallDelay = 1f;
    private float destroyDelay = 2f;

    [SerializeField] public Rigidbody2D rigidBody;


    private IEnumerator fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.gravityScale = 0.8f;
        Destroy(gameObject, destroyDelay);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(fall());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
