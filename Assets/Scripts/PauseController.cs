using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; } = false;

    // Unity automatically runs this the moment the scene loads
    private void Start()
    {
        // Force the game to be unpaused on load
        SetPause(false);
        
        // Also force time to run normally, just in case!
        Time.timeScale = 1f; 
    }

    public static void SetPause(bool pause)
    {
        IsGamePaused = pause;
        
        // If your game relies on timeScale to freeze enemies/animations, 
        // it's good practice to handle it right inside this method:
        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}