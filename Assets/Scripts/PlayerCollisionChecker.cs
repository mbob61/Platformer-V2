using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionChecker : MonoBehaviour
{
    private bool collidingWithGround;
    public LayerMask groundLayer;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(groundLayer))
        {
            collidingWithGround = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(groundLayer))
        {
            collidingWithGround = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(groundLayer))
        {
            collidingWithGround = false;
        }
    }

    public bool isColliding()
    {
        return collidingWithGround;
    }
}
