using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0.0f, 0.0f, -10f);
    private float smoothTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform objectToFollow;

    // Update is called once per frame
    void LateUpdate()
    {
        if (objectToFollow != null)
        {
            Vector3 targetPosition = objectToFollow.position + offset;

            if (targetPosition.y > -2.5 && targetPosition.y < 2.5f)
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, targetPosition.y, transform.position.z), ref velocity, smoothTime);
            }
            if (targetPosition.x > 4 && targetPosition.x < 28f)
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetPosition.x, transform.position.y, transform.position.z), ref velocity, smoothTime);
            }
        }

    }
}
