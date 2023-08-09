using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceTestController : MonoBehaviour
{
    float horizontal, vertical;
    float defaultHorizontal, defaultVertical;
    float maxForce;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultHorizontal = 3.0f;
        defaultVertical = 3.0f;
        maxForce = 10.0f;

        horizontal = defaultHorizontal;
        vertical = defaultVertical;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(5.0f, 5.0f), ForceMode2D.Impulse);
        }
    }
}
