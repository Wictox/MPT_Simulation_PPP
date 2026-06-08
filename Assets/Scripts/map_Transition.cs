using System;
using System.Collections; // We added this to allow Coroutines
using Unity.Cinemachine;
using UnityEngine;

public class map_Transition : MonoBehaviour
{
    [Header("Cinemachine & Boundaries")]
    [SerializeField] private PolygonCollider2D mapBoundry;
    private CinemachineConfiner2D confiner;
    
    [Header("Transition Direction")]
    [SerializeField] private Direction direction;
    [SerializeField] private Transform teleportTargetPosition;
    [SerializeField] private float transitionDistance = 2f;

    [Header("Fade Settings (NEW)")]
    [SerializeField] private CanvasGroup fadeScreen; 
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float waitInDark = 0.5f;

    // Making this static ensures if the player touches two doors at once, only one triggers!
    private static bool isTransitioning = false; 

    enum Direction { Up, Down, Left, Right, Teleport }

    private void Awake()
    {
        // Modern Unity way to find the confiner
        confiner = UnityEngine.Object.FindAnyObjectByType<CinemachineConfiner2D>();
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Instead of instant teleport, we start the Coroutine
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            StartCoroutine(TransitionRoutine(collision.gameObject));
        }
    }

    private IEnumerator TransitionRoutine(GameObject player)
    {
        isTransitioning = true;

        // 1. Fade OUT to Black
        if (fadeScreen != null)
        {
            while (fadeScreen.alpha < 1)
            {
                fadeScreen.alpha += Time.deltaTime * fadeSpeed;
                yield return null; // Wait a frame
            }
        }

        // --- THE TIME STOP ---
        // Everything below happens instantly while the screen is completely black

        // Update the camera's invisible wall
        if (confiner != null && mapBoundry != null)
        {
            confiner.BoundingShape2D = mapBoundry;
        }

        // Move the player using your exact original logic
        UpdatePlayerPosition(player);

        // Wait in the dark for a brief moment. 
        // This is crucial: it gives Cinemachine time to snap to the new boundary!
        yield return new WaitForSeconds(waitInDark);

        // ----------------------

        // 3. Fade IN to the new map
        if (fadeScreen != null)
        {
            while (fadeScreen.alpha > 0)
            {
                fadeScreen.alpha -= Time.deltaTime * fadeSpeed;
                yield return null; // Wait a frame
            }
        }

        isTransitioning = false;
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        if (direction == Direction.Teleport)
        {
            if (teleportTargetPosition != null)
            {
                player.transform.position = teleportTargetPosition.position;
            }
            else
            {
                Debug.LogError("Teleport Target Position is MISSING in the Inspector!");
            }
            return;
        }

        Vector3 newPos = player.transform.position;
        switch (direction)
        {
            case Direction.Up:    newPos.y += transitionDistance; break;
            case Direction.Down:  newPos.y -= transitionDistance; break;
            case Direction.Left:  newPos.x -= transitionDistance; break;
            case Direction.Right: newPos.x += transitionDistance; break;
        }
        player.transform.position = newPos;
    }
}