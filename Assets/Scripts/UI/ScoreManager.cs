using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private Transform player;
    private float startX;
    private bool scoring;

    public int CurrentScore { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!scoring) { return; }

        float distance = player.position.x - startX;
        CurrentScore = Mathf.Max(0, Mathf.FloorToInt(distance));
        UIManager.Instance.UpdateScore(CurrentScore);
    }

    public void StartScoring()
    {
        startX = player.position.x;
        CurrentScore = 0;
        scoring = true;
    }

    public void StopScoring()
    {
        scoring = false;
    }
}
