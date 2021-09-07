using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Player;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public TextMeshProUGUI minutesLabel;
    public TextMeshProUGUI secondsLabel;
    public TextMeshProUGUI accidentsLabel;

    public Transform playerPosition;
    public Transform gameOverTriggerPosition;

    public Slider progressBar;
    public Slider balanceBar;

    public GameObject[] starMiss;
    public GameObject[] clockMiss;

    Clock clock;
    ScoreManager scoreManager;
    private PlayerMovementController playerMovement;

    public float progressPercentage = 0;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        clock = FindObjectOfType<Clock>();
        playerMovement = FindObjectOfType<PlayerMovementController>();
    }

    private void Update()
    {
        minutesLabel.text = clock.Minutes.ToString("00");
        secondsLabel.text = clock.Seconds.ToString("00");
        accidentsLabel.text = scoreManager.fallNumber.ToString();

        if(scoreManager.clockNumber == 2)
        {
            clockMiss[clockMiss.Length - 1].SetActive(true);
        }
        else if (scoreManager.clockNumber == 1)
        {
            clockMiss[clockMiss.Length - 2].SetActive(true);
        }
        else if(scoreManager.clockNumber == 1)
        {
            clockMiss[clockMiss.Length - 3].SetActive(true);
        }

        if(scoreManager.starNumber == 2)
        {
            starMiss[0].SetActive(true);
        }
        else if(scoreManager.starNumber == 1)
        {
            starMiss[1].SetActive(true);
        }
        else if(scoreManager.starNumber == 0)
        {
            starMiss[2].SetActive(true);
        }


        progressPercentage = playerPosition.position.x / gameOverTriggerPosition.position.x;
        progressBar.value = progressPercentage;

        balanceBar.value = playerMovement.balanceAmount;
    }
}
