using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
        UpdateScore(0);
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
