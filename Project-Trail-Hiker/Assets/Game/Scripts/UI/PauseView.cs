using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.UI
{
    public class PauseView : Ui
    {
        //private GameManager.GameManager gameManager;

        void Start()
        {
            //gameManager = FindObjectOfType<GameManager.GameManager>();
        }

        public void ResumeButtonPressed()
        {
            GameManager.GameStart();
            this.gameObject.SetActive(false);
        }

        public void QuitButtonPressed()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("InitialScene");
        }
    }
}

