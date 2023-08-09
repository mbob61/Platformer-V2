using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float spriteLength, startPosition;
    [SerializeField] private Camera cam;
    [SerializeField] private float parallaxEffectAmount;

    void Start()
    {
        startPosition = transform.position.x;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float moveBoundary = cam.transform.position.x * (1 - parallaxEffectAmount);
        float distance = cam.transform.position.x * parallaxEffectAmount;

        transform.position = new Vector2(startPosition + distance, transform.position.y);

        //if (moveBoundary > startPosition + spriteLength) startPosition += spriteLength;
        //else if (moveBoundary < startPosition - spriteLength) startPosition -= spriteLength;
    }
}
