using Game.Scripts.Audio;
using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.GameManager
{
    public class GameManager : MonoBehaviour
    {
        public Canvas pauseUI;
        public Canvas gameOverUI;

        PlayerColliderManager playerColliderManager;

        private float levelSpeed;
        [SerializeField] private bool isGameRunning;
        private bool isPaused;

        public float LevelSpeed { get { return levelSpeed; } }
        public bool IsGameRunning => isGameRunning;

        public bool IsPaused => isPaused;

        private void Awake()
        {
            isGameRunning = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            GameStart();
            playerColliderManager = FindObjectOfType<PlayerColliderManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = true;
                Time.timeScale = 0.0f;
                isGameRunning = false;
                pauseUI.gameObject.SetActive(true);
            }

            if (playerColliderManager.isGameOver)
            {
                GameOver();
            }
        }

        public void GameStart()
        {
            isPaused = false;
            isGameRunning = true;
            levelSpeed = 1.0f;
            Time.timeScale = 1.0f;
        }

        public void GameOver()
        {
            isPaused = false;
            Time.timeScale = 0.0f;
            Debug.Log("GameOver");
            isGameRunning = false;
            gameOverUI.gameObject.SetActive(true);
        }

        public void DisableGameRunning()
        {
            isGameRunning = false;
        }

        public void DisablePause()
        {
            isPaused = false;
        }
    }
}
