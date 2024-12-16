using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class YuniKartGameManager : MonoBehaviour
{
    public static YuniKartGameManager instance;
    public TextMeshProUGUI lapText;
    private int lapCount = 0;
    public GameObject gameOverCanvas;

    public int coins = 0;
    public int score = 0;
    public TextMeshProUGUI scoreTxt;

    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvas.SetActive(false);
        UpdateLapText();
        UpdateScoreTxt();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerCrossedFinishLine() 
    { 
        if (lapCount > 0) // Ignore the first crossing as it is the start 
        { 
            lapCount++; 
            UpdateLapText(); 
            if (lapCount >= 3) 
            { 
                EndGame(); 
            } 
        } 
        else 
        { 
            lapCount++; // Increment for the first crossing to start counting laps 
        } 
    } 

    private void UpdateLapText() 
    { 
        lapText.text = "Laps: " + lapCount.ToString(); 
    } 
    
    private void EndGame() 
    { 
        gameOverCanvas.SetActive(true); // Disable player controls or other game-ending logic here // Example: 
        GetComponent<WheelController>().enabled = false; // Optionally stop the stopwatch if you have one running // 
    } 
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreTxt();
    }

    private void UpdateScoreTxt()
    {
        scoreTxt.text = score.ToString();
    }
}
