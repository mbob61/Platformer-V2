using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBridge : MonoBehaviour
{
    private float health;
    // Start is called before the first frame update
    void Start()
    {
        health = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        print(health);
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        print(collision.gameObject);
        print(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            health -= Time.deltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
    }
}
