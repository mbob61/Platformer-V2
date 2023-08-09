using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLadderMovement : MonoBehaviour
{
    private float vertical;
    private float speed = 8.0f;
    private bool nextToLadder = false;
    private bool isClimbing = false;

    [SerializeField] private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        //player = GetComponent<PlayerV3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nextToLadder && Mathf.Abs(vertical) > 0.0f)
        {
            isClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            print("im climbing");
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, vertical * speed);
        }
        //else
        //{
        //    if (!player.getIsDashing() || !player.getIsHovering())
        //    {
        //        rigidBody.gravityScale = GetComponent<PlayerV3>().getGravityScale();
        //    }
        //}
    }

    public void verticalMove(InputAction.CallbackContext context)
    {
        vertical = context.ReadValue<Vector2>().y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            nextToLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            nextToLadder = false;
            isClimbing = false;
        }   
    }
}
