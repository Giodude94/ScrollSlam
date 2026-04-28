using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;
using TMPro.EditorUtilities;
using System.Transactions;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;

    private float score;
    private float bestScore;

    public float Score => score;
    public float BestScore => bestScore;

    public enum GameState
    {
        WaitingToStart,
        Running,
        GameOver
    }

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentState = GameState.WaitingToStart;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        bestScore = PlayerPrefs.GetFloat("BestScore", 0f);
    }

    private void Update()
    {
        if (CurrentState == GameState.Running) 
        {
            score += Time.deltaTime;
        }
    }

    public void StartRun()
    {
        score = 0f;
        CurrentState = GameState.Running;
    }

    public void GameOver()
    {
        if (CurrentState == GameState.GameOver)
        {
            return;
        }

        CurrentState = GameState.GameOver;

        if (score > bestScore) 
        {
            bestScore = score;
            PlayerPrefs.SetFloat("BestScore", bestScore);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        SendScoreEvent();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void SendScoreEvent()
    {
        GameEvent e = new GameEvent 
        {
            //playerId = ,
            //sessionId = 
        };
    }
}
