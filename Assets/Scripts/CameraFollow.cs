using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;      // Reference to the player
    public float followThreshold; // Distance from the center where camera starts following
    private Vector3 offset;      // Initial offset between camera and player

    void Start()
    {
        // Calculate initial offset
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        Vector3 playerPosition = player.position;
        Vector3 cameraPosition = transform.position;
        Vector3 screenCenter = cameraPosition + offset;

        if (Mathf.Abs(playerPosition.x - screenCenter.x) > followThreshold)
        {
            transform.position = new Vector3(playerPosition.x + offset.x, transform.position.y, transform.position.z);
        }
    }
}
