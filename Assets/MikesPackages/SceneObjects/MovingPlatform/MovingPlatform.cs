using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float speed = 3.0f;
    private int index;
    [SerializeField] Rigidbody2D rigidBody;

    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ( Vector2.Distance(transform.position,waypoints[index].position) < 0.05f)
        {
            index++;
            if (index >= waypoints.Count)
            {
                index = 0;
            }
        }
        else
        {
            Vector2 targetDirection = (waypoints[index].position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, waypoints[index].position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
