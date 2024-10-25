using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float runSpeed = 7f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer playerSprite;

    private bool isGrounded;
    private bool isCollidingWithObstacle; // variable to track if the player has collided with an obstacle

    public GameObject playerScoreHealth; // Reference to the UI that holds the Player's score and health
    public int maxHealth = 100;           // Maximum health
    public int currentHealth = 100;       // Current health
    public int damageLevel = 10;          // Damage level

    public float score = 0f;              // Player's score
    public int fuelCansCollected = 0;     // Fuel cans collected count

    public float scoreIncreaseRate = 9f;  // Score increase per second (9 meters per second)
    public Text scoreText;                // UI text for score
    public Text fuelCanText;              // UI text for fuel cans count
    public Image healthBarImg;            // Image for the health bar


    public GameObject gameOverPanel;      // UI panel for Game Over (displayed when player dies)
    public GameObject pausePanel;         // UI panel for Pause Menu
    //private float gameOverDelay = 4f;
    public GameObject endGameScoreboard;

    private bool isRunning = true;        // Is the player running?
    public bool isGameOver = false;      // Is the game over?
    private bool isPaused = false;        // Is the game paused?

    private AudioSource backgroundMusic; // Reference to the AudioSource component

    private AudioSource heroSound; // Reference to the AudioSource component
    public AudioClip heroHurtSound; // Sound clip for when the hero is hurt
    public AudioClip heroJumpSound; // Sound clip for when the hero jumps
    public AudioClip heroCollideSound; // Sound clip for when the hero collides with an obstacle

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D component
        animator = GetComponent<Animator>(); // Get Animator component

        playerSprite = GetComponent<SpriteRenderer>();

        heroSound = GetComponent<AudioSource>();

        // Initialize health
        currentHealth = maxHealth;

        // Initialize the UI
        UpdateUI();

        // Start increasing the score
        StartCoroutine(IncreaseScore());

        // Find the AudioSource for background music which is a GameObject called "BackgroundMusic"
        backgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();

        //Reset the score and fuel cans collected in PlayerPrefs
        PlayerPrefs.SetInt("DistanceRan", 0);
        PlayerPrefs.SetInt("FuelCansCollected", 0);
    }

    void Update()
    {
        // Check for game over conditions or key press
        if (isGameOver)
        {
            ShowScoreboard();

            // Freeze everything
            ///Time.timeScale = 0f;
            //return;

        }

        if (!isCollidingWithObstacle)
        {
            // Constant horizontal movement (Running)
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);

            // Set animation for running
            animator.SetBool("isRunning", true);
        }

        // Jumping logic
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            // Play the jump sound
            heroSound.PlayOneShot(heroJumpSound);

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            animator.SetBool("isJumping", true); // Trigger jump animation
            isGrounded = false; // Disable further jumps until the player lands

            isCollidingWithObstacle = false; // Reset the obstacle flag
        }

        // Sliding logic when down arrow key is pressed
        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded)
        {
            animator.SetBool("isSliding", true); // Trigger slide animation
            // running false
            animator.SetBool("isRunning", false);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) && isGrounded)
        {
            animator.SetBool("isSliding", false); // Reset slide animation
            // running true
            animator.SetBool("isRunning", true);
        }

        //Pause the game when the player presses the Space key or the Escape key
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }


        // If player is in the air and not jumping, set jump animation to false
        if (rb.velocity.y < 0)
        {
            animator.SetBool("isJumping", false);
        }

        UpdateUI();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Crate"))
        {
            isGrounded = true; // Player has landed on the ground
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Play the collide sound
            heroSound.PlayOneShot(heroCollideSound);

            //Fall to ground by changing the player's velocity
            rb.velocity = new Vector2(rb.velocity.x, -jumpForce);

            isGrounded = true; // Player has landed on the ground
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Play the collide sound
            heroSound.PlayOneShot(heroCollideSound);

            isCollidingWithObstacle = true;
            isRunning = false; // Player is no longer running

            rb.velocity = Vector2.zero; // Stop player movement
            animator.SetBool("isStanding", true);

            isGrounded = true; // Player is standing on the crate
        }

        if (collision.gameObject.name == "FuelCan")
        {
            CollectFuelCan();
        }

        if (collision.gameObject.CompareTag("Laser") || collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(damageLevel);

            // Play the hurt sound
            heroSound.PlayOneShot(heroHurtSound);

            if (!isGameOver)
                StartCoroutine(PlayerFlicker());
        }

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Laser") || collision.gameObject.CompareTag("Bullet"))
        {
            //animator.SetBool("isFallen", true); // Assuming you have a falling animation
            //rb.velocity = Vector2.zero; // Stop player movement
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Laser") || collision.gameObject.CompareTag("Bullet"))
        {
            //animator.SetBool("isFallen", false);
            //WaitforSeconds(1);
            animator.SetBool("isRunning", true);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isCollidingWithObstacle = false; // Player is no longer touching the crate
            animator.SetBool("isStanding", false);
            animator.SetBool("isRunning", true);
        }

        isRunning = true; // Player can run again
    }

    IEnumerator PlayerFlicker()
    {
        for (int i = 0; i < 10; i++)
        {
            playerSprite.enabled = !playerSprite.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        playerSprite.enabled = true; // Ensure the sprite is visible at the end
    }

    private IEnumerator IncreaseScore()
    {
        while (!isGameOver)
        {
            if (isRunning)
            {
                // Increase score
                score += scoreIncreaseRate * Time.deltaTime * 3;

                // Update score in the UI
                UpdateUI();
            }
            yield return null;
        }
    }

    private void UpdateUI()
    {
        // Update score text if it's assigned
        if (scoreText != null)
        {
            scoreText.text = Mathf.FloorToInt(score).ToString();
        }

        // Update fuel can count text if it's assigned
        if (fuelCanText != null)
        {
            fuelCanText.text = fuelCansCollected.ToString();
        }

        // Update health bar UI if it's assigned
        if (healthBarImg != null)
        {
            SetHealth(currentHealth);
        }
    }

    public void SetHealth(int health)
    {
        //Change healthBarImg rgb color values based on health value. 70-100 is green, 30-69 is orange, 0-29 is red

        if (health >= 70)
        {
            healthBarImg.color = new Color32(0, 240, 34, 255);
        }
        else if (health >= 30)
        {
            healthBarImg.color = new Color32(255, 125, 0, 255);
        }
        else
        {
            // change to red
            healthBarImg.color = new Color32(255, 0, 0, 255);
        }

        // Set the health bar scale based on the health value

        float healthScale = (float)health / 100;
        healthBarImg.rectTransform.localScale = new Vector3(healthScale, 1f);
    }


    public void TakeDamage(int damage)
    {
        if (!isGameOver)
        {
            currentHealth = currentHealth - damage;

            //Debug.Log("Current Health " + currentHealth);

            // Check if health is below 0
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                GameOver();
            }

            UpdateUI();
        }
    }

    public void CollectFuelCan()
    {
        if (!isGameOver)
        {
            fuelCansCollected++;

            // Increase health if it's below the max value
            if (currentHealth < maxHealth)
            {
                currentHealth += 5;
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
            }

            UpdateUI();
        }
    }

    private void GameOver()
    {
        playerSprite.enabled = true; // Ensure the sprite is visible
        isGrounded = true; // Player is on the ground

        animator.SetBool("isFallen", true); // Assuming you have a falling animation
        rb.velocity = Vector2.zero; // Stop player movement

        // Show Game Over UI
        //gameOverPanel.SetActive(true);

        // Stop time and freeze everything
        Time.timeScale = 0f;

        // Make the player fall or stop (you can customize this as needed)
        // disable the player movement script
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        // Save the distance ran and fuel cans collected to PlayerPrefs
        PlayerPrefs.SetInt("DistanceRan", (int)score);
        PlayerPrefs.SetInt("FuelCansCollected", fuelCansCollected);

        isGameOver = true;
        isRunning = false;

        // Start the delay to show the scoreboard
        //Invoke("ShowScoreboard", gameOverDelay);
    }

    //Pause the game
    public void PauseGame()
    {
        // If already paused, resume the game
        if (isPaused)
        {
            isPaused = false;
            // Hide the pause panel
            Time.timeScale = 1f;
            backgroundMusic.UnPause();
        }
        else
        {
            isPaused = true;
            // Show the pause panel
            Time.timeScale = 0f;

            // Pause the background music
            if (backgroundMusic.isPlaying)
            {
                backgroundMusic.Pause();
            }
        }

        pausePanel.SetActive(isPaused);
    }

    private void ShowScoreboard()
    {
        // Call GameManager's GameOver method
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
        else
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }
}
