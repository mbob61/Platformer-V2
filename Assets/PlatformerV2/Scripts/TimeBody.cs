using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeBody : MonoBehaviour
{
    private bool doRewind;
    private bool isRewinding;

    private List<WorldPoint> worldPoints;
    private Rigidbody2D rigidBody;

    public float secondsToRecord = 5f;

    // Start is called before the first frame update
    void Start()
    {
        worldPoints = new List<WorldPoint>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doRewind)
        {
            startRewind();

        } else
        {
            stopRewind();
        }
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            rewind();
        } else
        {
            record();
        }
    }

    public void performRewind(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            doRewind = true;
        }

        if (context.canceled)
        {
            doRewind = false;
        }
    }

    private void startRewind()
    {
        isRewinding = true;
        rigidBody.isKinematic = true;
    }

    private void stopRewind()
    {
        isRewinding = false;
        rigidBody.isKinematic = false;
    }

    private void rewind()
    {
        if (worldPoints.Count > 0)
        {
            WorldPoint point = worldPoints[0];
            rigidBody.MovePosition(point.position);
            rigidBody.velocity = point.velocity;
            worldPoints.RemoveAt(0);
        } else
        {
            stopRewind();
        }
    }

    private void record()
    {
        if (worldPoints.Count > Mathf.Round(secondsToRecord / Time.fixedDeltaTime))
        {
            worldPoints.RemoveAt(worldPoints.Count - 1); 
        }
        worldPoints.Insert(0, new WorldPoint(transform.position, transform.localScale, rigidBody.velocity));
    }

    public bool getIsRewinding()
    {
        return isRewinding;
    }
}
