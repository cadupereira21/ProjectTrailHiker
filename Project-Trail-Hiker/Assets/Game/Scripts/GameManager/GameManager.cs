using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Canvas pauseUI;

    private float levelSpeed;
    [SerializeField] bool isGameRunning;

    public float LevelSpeed { get { return levelSpeed; } }
    public bool IsGameRunning { get { return isGameRunning; } }

    private void Awake()
    {
        isGameRunning = true;
    }

    // Start is called before the first frame update
    void Start()
    {

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
    }

    public void GameStart()
    {
        isGameRunning = true;
        levelSpeed = 1.0f;
        Time.timeScale = 1.0f;
    }

    public void GameOver()
    {
        isGameRunning = false;
    }
}
