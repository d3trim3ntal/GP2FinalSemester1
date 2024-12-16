using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // Reference to the Text component
    public int countdownTime = 3; // Countdown start time
    public GameObject countCanvas;
    public Button startButton;
    public Button restartButton;
    public GameObject postStart;
    public GameObject postGame;
    public StopWatch stopwatch;
    public GameObject watchImg;
    public AudioSource countdownAudioSource;
    public AudioSource initial;
    public AudioClip countdownEndClip;
    public AudioClip dayclip;
    public AudioClip buttonClick;

    private void Start()
    {
        countCanvas.gameObject.SetActive(false);
        startButton.onClick.AddListener(StartCountdown);
        restartButton.gameObject.SetActive(false);
        GetComponent<WheelController>().enabled = false;
        postStart.gameObject.SetActive(true);
        postGame.gameObject.SetActive(false);
        watchImg.gameObject.SetActive(false);
        stopwatch.enabled = false;
        initial.Play();
        initial.PlayOneShot(dayclip);
    }

    public void StartCountdown()
    { 
        initial.Stop();
        startButton.gameObject.SetActive(false);
        countCanvas.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(true);
        StartCoroutine((CountdownCoroutine()));
        if (countdownAudioSource != null && dayclip != null) 
        { 
            countdownAudioSource.PlayOneShot(buttonClick); 
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        while (countdownTime > 0)
        {
            GetComponent<WheelController>().enabled = false;
            countdownText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownText.text = "Go!";
        if (countdownAudioSource != null && countdownEndClip != null) 
        { 
            countdownAudioSource.PlayOneShot(countdownEndClip); 
        }

        yield return new WaitForSeconds(1f);

        // Remove the countdown UI
        countdownText.gameObject.SetActive(false);
        countCanvas.gameObject.SetActive(false);
        postStart.gameObject.SetActive(false);
        postGame.gameObject.SetActive(true);
        GetComponent<WheelController>().enabled = true;
        
        restartButton.gameObject.SetActive(true);

        stopwatch.enabled = true;
        watchImg.gameObject.SetActive(true);
        stopwatch.StartStopwatch();
    }

    private void Awake()
    {
        restartButton.onClick.AddListener(RestartScene);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

