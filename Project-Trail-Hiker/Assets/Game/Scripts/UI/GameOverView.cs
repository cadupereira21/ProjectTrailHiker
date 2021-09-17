using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.UI
{
    public class GameOverView : Ui
    {
        // private ScoreManager scoreManager;
        // private GameManager.GameManager gameManager;

        public TextMeshProUGUI timeScore;
        public TextMeshProUGUI starScore;

        public void Start()
        {
            // scoreManager = FindObjectOfType<ScoreManager>();
            // gameManager = FindObjectOfType<GameManager.GameManager>();

            starScore.text = ScoreManager.starNumber.ToString();
            timeScore.text = ScoreManager.clockNumber.ToString();
        }

        public void TryAgainButtonPressed()
        {
            gameObject.SetActive(false);
            GameManager.GameStart();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }

        public void QuitButtonPressed(){
            SceneManager.LoadScene("InitialScene");    
        }
    }
}
