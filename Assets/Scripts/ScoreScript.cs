using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    public static ScoreScript instance;

    [Header("scoring variables")]
    public int p1Score;
    public TMP_Text p1ScoreText;
    public int p2Score;
    public TMP_Text p2ScoreText;
    public int winScore = 5;
    public float speedModifierPerScore = 2f;

    [Header("restart & game nav variables")]
    public GameObject p1WinText;
    public GameObject p2WinText;
    public GameObject Menu;
    private int currentLevel;

    public void Awake()
    {
        instance = this;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        Menu.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p1Score = 0;
        p2Score = 0;
        UpdateScoreDisplay();
    }
    public void Update()
    {
        WinCondition();
    }

    public void AddScoreP1()
    {
        p1Score = p1Score + 1;
        UpdateScoreDisplay();

        UpdateBallSpeed(Side.Left, p1Score);
    }

    public void AddScoreP2()
    {
        p2Score = p2Score + 1;
        UpdateScoreDisplay();

        UpdateBallSpeed(Side.Right, p2Score);
    }

    public void UpdateScoreDisplay()
    {
        p1ScoreText.text = p1Score.ToString();
        p2ScoreText.text = p2Score.ToString();
    }

    public void WinCondition()
    {
        if (p1Score >= winScore)
        {
            //display P1 wins! screen
            Menu.SetActive(true);
            p1WinText.SetActive(true);
            p2WinText.SetActive(false);
        }
        if (p2Score >= winScore)
        {
            //display P2 wins! screen
            Menu.SetActive(true);
            p2WinText.SetActive(true);
            p1WinText.SetActive(false);
        }
        //update text to Marius' animations later
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(currentLevel);
    }

    void UpdateBallSpeed(Side side, int score)
    {
        float speedModifier = 1 + score * speedModifierPerScore;

        var gameManager = FindFirstObjectByType<GameManager>();
        gameManager.SetSpeedModifier(side, speedModifier);
    }

    /*public void backToMain()
    {
        //go back to main menu from game scene
    }*/

}
