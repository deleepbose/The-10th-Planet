using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f; // Speed of the bullet
    private Vector2 direction; // Direction in which the bullet will move
    private Rigidbody2D rb;    // Reference to the Rigidbody2D component

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        rb.velocity = direction * speed;  // Set the bullet velocity based on the given direction
    }

    // Set the direction for the bullet
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Destroy the bullet on collision with player or ground
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
