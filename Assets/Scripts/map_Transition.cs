using System;
using Unity.Cinemachine;
using UnityEngine;

public class map_Transition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapBoundry;
    private CinemachineConfiner2D confiner;
    
    [SerializeField] private Direction direction;
    [SerializeField] private Transform teleportTargetPosition;
    [SerializeField] private float transitionDistance = 2f;

    enum Direction { Up, Down, Left, Right, Teleport }

    private void Awake()
    {
        // Modern Unity way to find the confiner
        confiner = UnityEngine.Object.FindAnyObjectByType<CinemachineConfiner2D>();
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Update the camera's invisible wall
            if (confiner != null)
            {
                confiner.BoundingShape2D = mapBoundry;
            }

            // 2. Move the player
            UpdatePlayerPosition(collision.gameObject);
        }
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