using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class ScoreScript : MonoBehaviour
{
    public static ScoreScript instance;
    public bool VictoryAchived = false;

    [Header("scoring variables")]
    public int p1Score;
    public TMP_Text p1ScoreText;
    public int p2Score;
    public TMP_Text p2ScoreText;
    public int winScore = 5;
    public float speedModifierPerScore = 2f;
    int lastPoint;

    [Header("restart & game nav variables")]
    public GameObject p1WinText;
    public GameObject p2WinText;
    public GameObject Menu;
    private int currentLevel;

    AudioManager audioManager;
    bool isFinishPlaying = false;
    bool isKidsPlaying = false;

    public void Awake()
    {
        instance = this;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        Menu.SetActive(false);
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPoint = winScore - 1;
        p1Score = 0;
        p2Score = 0;
        UpdateScoreDisplay();
    }
    public void Update()
    {
        WinCondition();
        finishSFX();
    }

    public void AddScoreP1()
    {
        p1Score = p1Score + 1;
        UpdateScoreDisplay();

        UpdateBallSpeed(1, p1Score);
    }

    public void AddScoreP2()
    {
        p2Score = p2Score + 1;
        UpdateScoreDisplay();

        UpdateBallSpeed(2, p2Score);
    }

    public void UpdateScoreDisplay()
    {
        string score1Reformat;
        string score2Reformat;
        score1Reformat = p1Score.ToString();
        score2Reformat = p2Score.ToString();
        p1ScoreText.text = score1Reformat.PadLeft(2, '0');
        p2ScoreText.text = score2Reformat.PadLeft(2, '0');
        //testing a reformatting to have a leading zero in scoreboard. below is old way with only 1 digit.
        //p1ScoreText.text = p1Score.ToString();
        //p2ScoreText.text = p2Score.ToString();
    }

    void finishSFX()
    {
        //plays "finish him" sfx when one point left to win, plays too many time probably from being in update
        if(p1Score == lastPoint && !isFinishPlaying)
        {
            audioManager.PlaySFX(audioManager.Finish);
            isFinishPlaying = true;
        }
        if(p2Score == lastPoint && !isFinishPlaying)
        {
            audioManager.PlaySFX(audioManager.Finish);
            isFinishPlaying = true;
        }
    }    

    public void WinCondition()
    {
        if (p1Score >= winScore && !isKidsPlaying)
        {
            VictoryAchived = true;
            //display P1 wins! screen
            Menu.SetActive(true);
            p1WinText.SetActive(true);
            p2WinText.SetActive(false);
            audioManager.PlaySFX(audioManager.Kids); //this plays too many times probably from being in update
            isKidsPlaying = true;

        }
        if (p2Score >= winScore && !isKidsPlaying)
        {
            VictoryAchived = true;
            //display P2 wins! screen
            Menu.SetActive(true);
            p2WinText.SetActive(true);
            p1WinText.SetActive(false);
            audioManager.PlaySFX(audioManager.Kids);
            isKidsPlaying = true;
        }
        //update text to Marius' animations later
    }

    public void RestartGame()
    {
        if (!VictoryAchived) return;

        //for UI button restart on win/lose
        SceneManager.LoadScene(currentLevel);
    }

    void UpdateBallSpeed(int playerNumber, int score)
    {
        float speedModifier = 1 + score * speedModifierPerScore;

        var gameManager = FindFirstObjectByType<GameManager>();
        gameManager.SetSpeedModifier(playerNumber, speedModifier);
    }

    public void backToMain()
    {
        if (!VictoryAchived) return;

        //go back to main menu on win/lose
        SceneManager.LoadScene(0);
    }

}
