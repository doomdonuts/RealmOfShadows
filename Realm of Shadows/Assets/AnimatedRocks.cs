using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedRocks : MonoBehaviour
{
    public float minY;
    public float maxY;

    public float speed = 0.1f;
    public int direction = 1; // 1 is up, -1 is down
    private Vector3 startingPosition;
    private Vector3 finalPosition;

    void Start()
    {
        startingPosition = transform.position;
        startingPosition.y = minY;
        finalPosition = transform.position;
        finalPosition.y = maxY;
    }

    void FixedUpdate()
    {
        Vector3 lerpPosition = new Vector3();
        if (direction == 1)
        {
            lerpPosition = Vector3.Lerp(transform.position, finalPosition, speed);

        } else if (direction == -1)
        {
            lerpPosition = Vector3.Lerp(transform.position, startingPosition, speed);
        }


        lerpPosition.z = 0;
        transform.position = lerpPosition;

        if (transform.position.y >= maxY - speed * 1.5) direction = -1;
        if (transform.position.y <= minY + speed * 1.5) direction = 1;

    }

}
