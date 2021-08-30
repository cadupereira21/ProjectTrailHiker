using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("InitialScene");
    }
}

