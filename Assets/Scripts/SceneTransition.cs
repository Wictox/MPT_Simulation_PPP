using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // We need this to load scenes!

public class SceneTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    public AudioSource musicToFade;
    public CanvasGroup blackScreen;
    public float transitionTime = 2.0f;
    
    [Header("Next Scene")]
    public string nextSceneName = "SampleScene"; // Put your actual game scene name here!

    // Call this from your Start Button
    public void StartGame()
    {
        // Turn on Raycasts so the player can't spam the start button while it fades
        blackScreen.blocksRaycasts = true; 
        
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        float timer = 0f;
        float startVolume = musicToFade != null ? musicToFade.volume : 1f;

        // Loop until our timer reaches the transitionTime (2 seconds)
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            float progress = timer / transitionTime; // Goes from 0.0 to 1.0

            // 1. Fade OUT the music
            if (musicToFade != null) 
            {
                musicToFade.volume = Mathf.Lerp(startVolume, 0f, progress);
            }

            // 2. Fade IN the black screen
            if (blackScreen != null) 
            {
                blackScreen.alpha = Mathf.Lerp(0f, 1f, progress);
            }

            yield return null; // Wait for the next frame
        }

        // 3. NOW that it's pitch black and silent, load the game!
        SceneManager.LoadScene(nextSceneName);
    }
}