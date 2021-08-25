﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Primeira fase: 3 Relógios - 2m30 | 2 Relógios - 3m30 | 1 Relógio - 5m | 3 Estrelas - cair 2 vezes | 2 Estrelas - cair 5 vezes | 1 Estrela - cair 10 vezes
    public Clock clock;

    private int clockNumber = 3;
    private int starNumber = 3;
    private int fallNumber = 0;

    public int FallNumber { get { return fallNumber; } set { fallNumber = default; } }

    // Update is called once per frame
    void Update()
    {
        if(clock.Minutes < 2 || clock.Seconds < 30.0f) { clockNumber = 3; } 
        else if(clock.Minutes < 3 || clock.Seconds < 30.0f) { clockNumber = 2; }
        else if (clock.Minutes < 5) { clockNumber = 1; }
        else { clockNumber = 0; }

        if(fallNumber <= 2) { starNumber = 3; }
        else if (fallNumber <= 5) { starNumber = 2; }
        else if(fallNumber <= 10) { starNumber = 1;  }  
        else { starNumber = 0; }

        Debug.Log("Numero de Relógios: " + clockNumber + "\n" + "Numero de Estrelas: " + starNumber);
    }
}
