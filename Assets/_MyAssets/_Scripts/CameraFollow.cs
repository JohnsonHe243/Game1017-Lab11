using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform.
    private float leftBound;
    private float rightBound;

    void Start()
    {
        // Calculate the boundaries for camera movement.
        float cameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
        leftBound = -cameraWidth / 3f;
        rightBound = cameraWidth / 3f;
    }

    void LateUpdate()
    {
        // Get player and camera positions.
        Vector3 playerPosition = target.position;
        Vector3 newPosition = transform.position;

        // Calculate new bounds.
        float newLeftBound = newPosition.x + leftBound;
        float newRightBound = newPosition.x + rightBound;

        // Check if the player has moved into the left 1/3 zone.
        if (playerPosition.x <= newLeftBound)
        {
            transform.position = new Vector3(playerPosition.x - leftBound, newPosition.y, newPosition.z);
        }
        // Check if the player has moved into the right 1/3 zone.
        else if (playerPosition.x >= newRightBound)
        {
            transform.position = new Vector3(playerPosition.x - rightBound, newPosition.y, newPosition.z);
        }
    }
}
