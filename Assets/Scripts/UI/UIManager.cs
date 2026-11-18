using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI totalCoinsText;

    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Buttons")]
    [SerializeField] private Button shopButton;


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

        coinsText.text = "Coins: " + CoinManager.Instance.GetCurrentCoins().ToString();

        totalCoinsText.text = "Total Coins: " + CoinManager.Instance.GetTotalCoins().ToString();


        if (GameManager.Instance.CurrentState == GameManager.GameState.GameOver)
        {
            if (!gameOverPanel.activeSelf)
            {
                gameOverPanel.SetActive(true);
            }
        }
        else if (GameManager.Instance.CurrentState.Equals(GameManager.GameState.Running))
        {
            if (shopButton.gameObject.activeSelf)
            {
                shopButton.gameObject.SetActive(false);
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
