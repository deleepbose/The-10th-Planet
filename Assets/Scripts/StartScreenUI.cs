using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenUI : MonoBehaviour
{
    public void OnStartGameClicked()
    {
        // Loads the PlayingLevel scene when the start button is clicked
        SceneManager.LoadScene("PlayingLevel");
    }
}
