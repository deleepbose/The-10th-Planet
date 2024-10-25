using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;  // Singleton instance

    public GameObject startUI;       // Start screen UI
    public GameObject inGameUI;      // In-game UI (score, stats)
    public GameObject gameOverUI;    // Game Over screen UI

    private void Start()
    {
        // Start with StartUI active, and others inactive
        ShowStartScreen();
    }

    // This method is called when the Start button is clicked
    public void StartGame()
    {
        startUI.SetActive(false);  // Hide the start screen
        inGameUI.SetActive(true);  // Show the in-game UI
        gameOverUI.SetActive(false);

        Time.timeScale = 1;  // Unpause the game 
    }

    // This method is called when the player dies or game ends
    public void GameOver()
    {
        inGameUI.SetActive(false);  // Hide in-game UI
        gameOverUI.SetActive(true); // Show the game over screen

        Time.timeScale = 0;  // Pause the game
    }

    // This method is called when the Play Again button is clicked
    public void RestartGame()
    {
        startUI.SetActive(true);   // Show start screen
        gameOverUI.SetActive(false);  // Hide the game over screen
    }

    private void ShowStartScreen()
    {
        startUI.SetActive(true);   // Show start screen
        inGameUI.SetActive(false); // Hide in-game UI
        gameOverUI.SetActive(false); // Hide game over screen

        Time.timeScale = 0;  // Pause the game until it starts
    }

    private void Update()
    {
        // Quit the game on pressing 'Q'
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();  // Quit the game on pressing 'Q'
        }
    }
}
