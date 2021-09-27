using System;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.GameManager
{
    public class ScoreManager : MonoBehaviour
    {
        // Primeira fase: 3 Relógios - 2m30 | 2 Relógios - 3m30 | 1 Relógio - 5m | 3 Estrelas - cair 2 vezes | 2 Estrelas - cair 5 vezes | 1 Estrela - cair 10 vezes
        public Clock clock;
        [SerializeField] private Fase[] fases;

        private int faseNumber;

        public int ClockNumber { private set; get; } = 3;
        public int StarNumber { private set; get; } = 3;
        public int fallNumber = 0;

        private void Start()
        {
            faseNumber = Int32.Parse(SceneManager.GetActiveScene().name.Substring(5));
        }

        // Update is called once per frame
        public void Update()
        {
            if (fases == null)
            {
                Debug.LogWarning("variable fase is not set");
                return;
            }
            if(clock.Minutes < fases[faseNumber-1].MaxTimeScore[0] || clock.Seconds < fases[faseNumber-1].MaxTimeScore[1]) { ClockNumber = 3; } 
            else if(clock.Minutes < fases[faseNumber-1].AverageTimeScore[0] || clock.Seconds < fases[faseNumber-1].AverageTimeScore[1]) { ClockNumber = 2; }
            else if (clock.Minutes < fases[faseNumber-1].MinTimeScore[0] || clock.Seconds < fases[faseNumber-1].MinTimeScore[1]) { ClockNumber = 1; }
            else { ClockNumber = 0; }

            if(fallNumber <= fases[faseNumber-1].MaxStarScore) { StarNumber = 3; }
            else if (fallNumber <= fases[faseNumber-1].AverageStarScore) { StarNumber = 2; }
            else if(fallNumber <= fases[faseNumber-1].MinStarScore) { StarNumber = 1;  }  
            else { StarNumber = 0; }
        }
    }
}
