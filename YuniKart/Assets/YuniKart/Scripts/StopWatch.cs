using System.Collections;
using UnityEngine;
using TMPro;

public class StopWatch : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to the UI Text component
    private float elapsedTime = 0f; // The elapsed time
    private bool isRunning = false; // Is the stopwatch running?

    private void Update()
    {
        if (isRunning)
        {
            // Increment the elapsed time by the time that passed since the last frame
            elapsedTime += Time.deltaTime;

            // Format the elapsed time to minutes, seconds, and milliseconds
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime % 60F);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 100F) % 100F);

            // Display the formatted time
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }

    public void StartStopwatch()
    {
        isRunning = true;
    }

    public void StopStopwatch()
    {
        isRunning = false;
    }

    public void ResetStopwatch()
    {
        isRunning = false;
        elapsedTime = 0f;
        timerText.text = "00:00:00";
    }
}

