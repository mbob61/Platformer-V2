using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private Rigidbody2D rigidBody;
    private int index = 0;

    void FixedUpdate()
    {
        if (waypoints.Count > 0)
        {
            if (Vector2.Distance(transform.position, waypoints[index].position) < 0.05f)
            {
                index++;
                if (index >= waypoints.Count)
                {
                    index = 0;
                }
                // Put this in later
                flipPlayersDirection();
            }
            else
            {
                Vector2 targetDirection = (waypoints[index].position - transform.position).normalized;
                rigidBody.velocity = new Vector2(Mathf.Max(targetDirection.x * speed, 1.0f), 0.0f);
                print(rigidBody.velocity.x);
            }
        }
    }

    private void flipPlayersDirection()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
