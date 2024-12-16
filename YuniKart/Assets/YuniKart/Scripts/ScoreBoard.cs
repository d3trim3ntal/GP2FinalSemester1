using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceScoreboard : MonoBehaviour
{
    public TextMeshProUGUI distanceText; // Reference to the UI Text component
    public Transform player; // Reference to the player's transform
    private Vector3 lastPosition; // Track the last position of the player
    private float totalDistance = 0f; // Total distance driven by the player

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        lastPosition = player.position;
        UpdateDistanceText();
    }

    private void Update()
    {
        // Calculate the distance moved since the last frame
        float distanceMoved = Vector3.Distance(player.position, lastPosition);
        totalDistance += distanceMoved;

        // Update the last position
        lastPosition = player.position;

        // Update the distance text
        UpdateDistanceText();
    }

    private void UpdateDistanceText()
    {
        distanceText.text = totalDistance.ToString("F2");
    }
}
