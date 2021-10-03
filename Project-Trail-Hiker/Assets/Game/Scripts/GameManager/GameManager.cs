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

        public float LevelSpeed { get { return levelSpeed; } }
        public bool IsGameRunning { get { return isGameRunning; } }

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
            // if (!isGameRunning)
            // {
            //     levelSpeed = 0.0f;
            //     Time.timeScale = 0.0f;
            // }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
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
            isGameRunning = true;
            levelSpeed = 1.0f;
            Time.timeScale = 1.0f;
        }

        public void GameOver()
        {
            Time.timeScale = 0.0f;
            Debug.Log("GameOver");
            isGameRunning = false;
            gameOverUI.gameObject.SetActive(true);
        }
    }
}
