using System.Collections;
using System.Collections.Generic;
using Game.Scripts.GameManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverView : MonoBehaviour
{
    ScoreManager scoreManager;
    GameManager gameManager;

    public TextMeshProUGUI timeScore;
    public TextMeshProUGUI starScore;

    public void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        gameManager = FindObjectOfType<GameManager>();

        starScore.text = scoreManager.starNumber.ToString();
        timeScore.text = scoreManager.clockNumber.ToString();
    }

    public void TryAgainButtonPressed()
    {
        gameObject.SetActive(false);
        gameManager.GameStart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void QuitButtonPressed(){
        SceneManager.LoadScene("InitialScene");    
    }
}
