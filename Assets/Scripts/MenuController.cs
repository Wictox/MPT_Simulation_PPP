using UnityEngine;
using UnityEngine.InputSystem; // You must add this namespace

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas; 

    void Start()
    {
        menuCanvas.SetActive(false); 
    }

    void Update()
    {
        // Check if the keyboard is connected to prevent errors
        if (Keyboard.current == null) return;

        // The new way to check for a single key press
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.tabKey.wasPressedThisFrame) 
        {
            if (!menuCanvas.activeSelf && PauseController.IsGamePaused)
            {
                return ;
            }
            menuCanvas.SetActive(!menuCanvas.activeSelf); 
            PauseController.SetPause(menuCanvas.activeSelf);
        }
    }
}