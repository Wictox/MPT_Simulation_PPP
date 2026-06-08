using System.Collections;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceHeight = 0.5f; 
    public float bounceDuration = 0.4f; 
    public int bounceCount = 3; 

    [Header("Spread Settings")]
    public float spreadDistance = 1.2f; // How far it bursts out of the chest

    public void StartBounce()
    {
        StartCoroutine(BounceHandler());
    }

    private IEnumerator BounceHandler()
    {
        // 1. Turn off the collider temporarily so the player can't grab it mid-air!
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Vector3 startPosition = transform.position;
        
        // 2. Pick a random spot around the chest for the item to eventually land
        Vector2 randomOffset = Random.insideUnitCircle * spreadDistance;
        Vector3 finalPosition = startPosition + (Vector3)randomOffset;

        float localHeight = bounceHeight; 
        float localDuration = bounceDuration; 
        
        Vector3 currentStart = startPosition;

        for (int i = 0; i < bounceCount; i++)
        {
            // Calculate where this specific bounce should land horizontally
            Vector3 nextTarget = Vector3.Lerp(startPosition, finalPosition, (float)(i + 1) / bounceCount);

            yield return Bounce(currentStart, nextTarget, localHeight, localDuration / 2); 
            
            currentStart = nextTarget; // The next bounce starts where this one ended
            localHeight *= 0.5f; 
            localDuration *= 0.8f; 
        }

        transform.position = finalPosition; // Snap exactly to the final spot
        
        // 3. Turn the collider back on so the player can collect it
        if (col != null) col.enabled = true;
    }

    private IEnumerator Bounce(Vector3 start, Vector3 end, float height, float halfDuration)
    {
        // --- UPWARDS PHASE ---
        float elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            float t = elapsedTime / halfDuration;
            
            // Move horizontally halfway to the target
            Vector3 currentHorizontal = Vector3.Lerp(start, Vector3.Lerp(start, end, 0.5f), t);
            
            // Add your vertical bounce
            transform.position = currentHorizontal + Vector3.up * Mathf.Lerp(0, height, t);
            
            elapsedTime += Time.deltaTime; 
            yield return null; 
        }

        // --- DOWNWARDS PHASE ---
        elapsedTime = 0f; 
        while (elapsedTime < halfDuration)
        {
            float t = elapsedTime / halfDuration;
            
            // Move horizontally the rest of the way
            Vector3 currentHorizontal = Vector3.Lerp(Vector3.Lerp(start, end, 0.5f), end, t);
            
            // Drop the vertical bounce back down
            transform.position = currentHorizontal + Vector3.up * Mathf.Lerp(height, 0, t);
            
            elapsedTime += Time.deltaTime; 
            yield return null; 
        }
    }
}