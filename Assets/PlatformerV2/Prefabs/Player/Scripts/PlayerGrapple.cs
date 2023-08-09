using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrapple : MonoBehaviour
{
    public LayerMask layer;
    public DistanceJoint2D distanceJoint;
    public LineRenderer lineRenderer;

    private Rigidbody2D rigidBody;
    private RaycastHit2D hit;
    private bool isCurrentlyGrappled;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        distanceJoint.enabled = false;
    }

    public void fireGrapple(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, layer);

            if (hit)
            {
                lineRenderer.SetPosition(0, transform.position);

                lineRenderer.SetPosition(1, hit.point);
                distanceJoint.connectedAnchor = hit.point;
                distanceJoint.enabled = true;
                lineRenderer.enabled = true;
                isCurrentlyGrappled = true;
            }
        }

        if (context.canceled)
        {
            distanceJoint.enabled = false;
            lineRenderer.enabled = false;
            isCurrentlyGrappled = false;
            //rigidBody.AddForce(new Vector2(rigidBody.velocity.x * 2.0f, rigidBody.velocity.y * 5.0f), ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        if (distanceJoint.enabled)
        {
            lineRenderer.SetPosition(0, transform.position);
            isCurrentlyGrappled = true;

        }
    }

    public bool isGrappled()
    {
        return isCurrentlyGrappled;
    }
}
