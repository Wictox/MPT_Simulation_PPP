using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes

public class ReturnToMenu : MonoBehaviour
{
    // Make sure this exactly matches the name of your menu scene!
    public string menuSceneName = "startscene"; 

    public void GoToMainMenu()
    {
        // Unpause the game just in case you use Time.timeScale = 0 for your pause menu
        Time.timeScale = 1f; 
        
        // Load the Start Menu
        SceneManager.LoadScene(menuSceneName);
    }
}