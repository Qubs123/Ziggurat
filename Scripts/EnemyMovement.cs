using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyDetection enemyDetection;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public Transform player;
    public Transform[] points;
    private int currentPoint;
    public bool isFollowingPlayer;

    void Start()
    {
        enemyDetection = GetComponent<EnemyDetection>();
        enemyDetection.OnPlayerDetected += FollowPlayer;
        currentPoint = 0;
    }

    void Update()
    {
        isFollowingPlayer = enemyDetection.isPlayerDetected;

        if (isFollowingPlayer)
        {
            FollowPlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        speed = 5;
        if (points.Length == 0) return; // Ensure there are waypoints

        Transform targetPoint = points[currentPoint];
        Vector3 direction = (targetPoint.position - transform.position).normalized;

        // Move and rotate towards the current waypoint
        MoveAndRotate(direction);

        // Check if close enough to the waypoint
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Move to the next waypoint
            currentPoint = (currentPoint + 1) % points.Length;
        }
    }

    void FollowPlayer()
    {
        speed = 7;
        Vector3 direction = (player.position - transform.position).normalized;
        MoveAndRotate(direction);
    }

    void MoveAndRotate(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        // Move in the direction
        transform.position += direction * speed * Time.deltaTime;

        // Calculate the target rotation based on movement direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the direction of movement
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
