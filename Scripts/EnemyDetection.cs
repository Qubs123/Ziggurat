using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public Transform player;
    public float detectionDistance = 10f;
    public float fovAngle = 45f; // Field of View Angle
    public LayerMask detectionLayer; // Layer for detection, typically the player layer
    public LayerMask obstacleLayer; // Layer for obstacles, e.g., walls

    public bool isPlayerDetected = false;
    public System.Action OnPlayerDetected;
    public float detectionHoldTime = 5;
    public float detectionTimer = 0;
    public Light detectionLight;
    public Color detectedColor = Color.red;
    public Color undetectedColor = Color.yellow;

    void Start()
    {
        detectionLight = GetComponentInChildren<Light>();
        detectionLight.color = undetectedColor;
    }

    void Update()
    {
        DetectPlayer();
        if (isPlayerDetected)
        {
            OnPlayerDetected?.Invoke();
        }
        detectionLight.color = isPlayerDetected ? detectedColor : undetectedColor;
    }

    void DetectPlayer()
    {
        // Calculate the direction from the enemy to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        
        // Calculate the angle between the enemy's forward direction and the direction to the player
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Check if the player is within the FOV and detection distance
        if (angleToPlayer < fovAngle / 2 && Vector3.Distance(transform.position, player.position) < detectionDistance)
        {
            // Perform a raycast to check if there are any obstacles between the enemy and the player
            if (!Physics.Raycast(transform.position, directionToPlayer, detectionDistance, obstacleLayer))
            {
                isPlayerDetected = true;
                detectionTimer = detectionHoldTime;
                return;
            }
        }

        if(detectionTimer > 0f)
        {
            detectionTimer -= Time.deltaTime;
        }

        if(detectionTimer <= 0f)
        {
            isPlayerDetected = false;
        }
    }
}