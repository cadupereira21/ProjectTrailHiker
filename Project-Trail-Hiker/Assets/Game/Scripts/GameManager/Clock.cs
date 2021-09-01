using System.Collections;
using System.Collections.Generic;
using Game.Scripts.GameManager;
using UnityEngine;

public class Clock : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] int minutes = 0;
    [SerializeField] float seconds = 0;
    [SerializeField] private float secondsLimit = 60f;

    public int Minutes { get { return minutes; } }
    public float Seconds { get { return seconds; } }

    //public Text minutesLabel;
    //public Text secondsLabel;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //secondsLabel.text = seconds.ToString("00");
        //minutesLabel.text = minutes.ToString("00");
        if (!gameManager.IsGameRunning)
        {
            return;
        }

        seconds += Time.deltaTime;

        if (seconds > secondsLimit)
        {
            minutes++;
            seconds = 0;
        }
    }
}
