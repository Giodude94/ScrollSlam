using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;


    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
    }

    private void Update()
    {
        if (GameManager.Instance == null) 
        {
            return;
        }

        scoreText.text = "Current Score: " + Mathf.FloorToInt(GameManager.Instance.Score).ToString();

        bestScoreText.text = "Best: " + Mathf.FloorToInt(GameManager.Instance.BestScore).ToString();

        if(GameManager.Instance.CurrentState == GameManager.GameState.GameOver)
        {
            if (!gameOverPanel.activeSelf)
            {
                gameOverPanel.SetActive(true);
            }
        }
    }
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void OnReplayPressed()
    {
        GameManager.Instance.Replay();
    }
}
