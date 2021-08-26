﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Canvas pauseUI;
    public Canvas gameOverUI;

    PlayerCheckpointDetection playerCheckpointDetection;

    private float levelSpeed;
    [SerializeField] bool isGameRunning;

    public float LevelSpeed { get { return levelSpeed; } }
    public bool IsGameRunning { get { return isGameRunning; } }

    private void Awake()
    {
        isGameRunning = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameStart();
        playerCheckpointDetection = FindObjectOfType<PlayerCheckpointDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameRunning)
        {
            levelSpeed = 0.0f;
            Time.timeScale = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGameRunning = false;
            pauseUI.gameObject.SetActive(true);
        }

        if (playerCheckpointDetection.isGameOver)
        {
            GameOver();
        }
    }

    public void GameStart()
    {
        isGameRunning = true;
        levelSpeed = 1.0f;
        Time.timeScale = 1.0f;
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        isGameRunning = false;
        gameOverUI.gameObject.SetActive(true);
    }
}