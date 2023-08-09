using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public float speed;
    public GameObject waypointA, waypointB;
    private bool moveLeft;
    private Vector3 desiredWaypoint;

    void Start()
    {
        moveLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveLeft)
        {
            desiredWaypoint = waypointA.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, waypointA.transform.position, speed * Time.deltaTime);
        }
        else
        {
            desiredWaypoint = waypointB.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, waypointB.transform.position, speed * Time.deltaTime);
        }

        if (transform.position == desiredWaypoint)
        {
            moveLeft = !moveLeft;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
}
