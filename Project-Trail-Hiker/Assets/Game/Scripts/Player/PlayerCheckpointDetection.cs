using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpointDetection : MonoBehaviour
{
    public bool isGameOver { private set; get; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("GameOverTrigger")){
            isGameOver = true;
        }
    }
}
