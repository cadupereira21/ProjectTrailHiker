using UnityEngine;

namespace Game.Scripts.UI
{
    public class Ui :MonoBehaviour
    {
        protected ScoreManager ScoreManager { private set; get; }
        protected GameManager.GameManager GameManager { private set; get; }

        protected virtual void Awake()
        {
            ScoreManager = FindObjectOfType<ScoreManager>();
            GameManager = FindObjectOfType<GameManager.GameManager>();
        }
    }
}