using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 cameraOffset;
    public float cameraSpeed = 0.1f;

    void Start()
    {   
        Vector3 startingPosition = player.position + cameraOffset;
        startingPosition.z = -10;
        transform.position = startingPosition;
    }

    void FixedUpdate()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;
        float rounded_x = RoundToNearestPixel(player_x);
        float rounded_y = RoundToNearestPixel(player_y);
        Vector3 roundedPosition = new Vector3(rounded_x, rounded_y, -10.0f);

        Vector3 finalPosition = roundedPosition + cameraOffset;
        Vector3 lerpPosition = Vector3.Lerp(transform.position, finalPosition, cameraSpeed);
        lerpPosition.z = -10;
        transform.position = lerpPosition;
    }

    public float pixelToUnits = 16f;

    public float RoundToNearestPixel(float unityUnits)
    {
        float valueInPixels = unityUnits * pixelToUnits;
        valueInPixels = Mathf.Round(valueInPixels);
        float roundedUnityUnits = valueInPixels * (1 / pixelToUnits);
        return roundedUnityUnits;
    }
}
