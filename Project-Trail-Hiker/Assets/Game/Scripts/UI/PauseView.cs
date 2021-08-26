using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseView : MonoBehaviour
{
    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void ResumeButtonPressed()
    {
        gameManager.GameStart();
        this.gameObject.SetActive(false);
    }

    public void QuitButtonPressed()
    {
        Application.Quit();
        Debug.Log("Quitting application!");
    }
}

