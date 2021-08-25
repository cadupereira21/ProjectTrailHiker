using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] int minutes = 0;
    [SerializeField] float seconds = 0;
    [SerializeField] private float secondsLimit = 60f;

    public int Minutes { get { return minutes; } }
    public float Seconds { get { return seconds; } }

    //public Text minutesLabel;
    //public Text secondsLabel;

    // Update is called once per frame
    void Update()
    {
        //secondsLabel.text = seconds.ToString("00");
        //minutesLabel.text = minutes.ToString("00");
        seconds += Time.deltaTime;

        if (seconds > secondsLimit)
        {
            minutes++;
            seconds = 0;
        }
    }
}
