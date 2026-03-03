using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;
using TMPro.EditorUtilities;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public enum GameState
    {
        Idle,
        Running,
        GameOver
    }

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetState(GameState.Idle);
    }

    public void StartRun()
    {
        SetState(GameState.Running);
        ScoreManager.Instance.StartScoring();
    }

    public void GameOver()
    {
        if (CurrentState == GameState.GameOver) { return; }

        SetState(GameState.GameOver);
        ScoreManager.Instance.StopScoring();
        UIManager.Instance.ShowGameOver();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;
    }
}
