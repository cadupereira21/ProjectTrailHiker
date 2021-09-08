using Game.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class Hud : MonoBehaviour
    {
        public TextMeshProUGUI minutesLabel;
        public TextMeshProUGUI secondsLabel;
        public TextMeshProUGUI accidentsLabel;

        public Transform playerPosition;
        public Transform gameOverTriggerPosition;

        public Slider progressBar;
        public Slider balanceBar;

        public GameObject[] starMiss;
        public GameObject[] clockMiss;
        public GameObject[] qteButtons;

        private Clock clock;
        private ScoreManager scoreManager;
        private PlayerMovementController playerMovement;
        private PlayerColliderManager playerCollider;

        private float progressPercentage = 0;

        private void Start()
        {
            scoreManager = FindObjectOfType<ScoreManager>();
            clock = FindObjectOfType<Clock>();
            playerMovement = FindObjectOfType<PlayerMovementController>();
            playerCollider = FindObjectOfType<PlayerColliderManager>();
        }

        private void Update()
        {
            minutesLabel.text = clock.Minutes.ToString("00");
            secondsLabel.text = clock.Seconds.ToString("00");
            accidentsLabel.text = scoreManager.fallNumber.ToString();

            switch (scoreManager.clockNumber)
            {
                case 2:
                    clockMiss[clockMiss.Length - 1].SetActive(true);
                    break;
                case 1:
                    clockMiss[clockMiss.Length - 2].SetActive(true);
                    break;
                default:
                {
                    if(scoreManager.clockNumber == 1)
                    {
                        clockMiss[clockMiss.Length - 3].SetActive(true);
                    }

                    break;
                }
            }

            switch (scoreManager.starNumber)
            {
                case 2:
                    starMiss[0].SetActive(true);
                    break;
                case 1:
                    starMiss[1].SetActive(true);
                    break;
                case 0:
                    starMiss[2].SetActive(true);
                    break;
            }


            progressPercentage = playerPosition.position.x / gameOverTriggerPosition.position.x;
            progressBar.value = progressPercentage;

            balanceBar.value = playerMovement.balanceAmount;
        }

        public void ShowQTEButton(string whichButton)
        {
            qteButtons[0].SetActive(true);
            var imageRenderer = qteButtons[0].GetComponent<Image>();
            imageRenderer.color = Color.white;
        }

        public void HideQTEButton(string whichButton)
        {
            qteButtons[0].SetActive(false);
        }

        public void QteRespondPlayerInput(string whichButton, bool isRightButton)
        {
            if (!qteButtons[0].activeInHierarchy) return;
            
            var imageRenderer = qteButtons[0].GetComponent<Image>();
            switch (isRightButton)
            {
                case true:
                    imageRenderer.color = Color.green;
                    break;
                case false:
                    imageRenderer.color = Color.red;
                    break;
            }
        }
    }
}
