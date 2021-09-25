using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Game.Scripts.UI
{
    public class GameOverView : Ui
    {
        /*
            - clocks : GameObject[3]
            - stars : GameObject[3]
            - newBottles : TextMeshProUGUI
            - newTotalBottles : TextMeshProUGUI
            - oldTotalBottles : TextMeshProUGUI
            - levelNumber : int 
         */

        [SerializeField] private GameObject[] clocks = new GameObject[3];
        [SerializeField] private GameObject[] stars = new GameObject[3];
        [SerializeField] private TextMeshProUGUI newBottles;
        
        [FormerlySerializedAs("newTotalBottles")] 
        [SerializeField] private TextMeshProUGUI newTotalBottlesLabel;
        
        private int newTotalBottles;
        private int oldTotalBottles;
        [SerializeField] private TextMeshProUGUI levelNumber;

        public void Start()
        {
            oldTotalBottles = PlayerPrefs.GetInt("Bottle");
            
            for (int i = 0; i < ScoreManager.clockNumber; i++)
                clocks[i].gameObject.SetActive(true);

            for (int i = 0; i < ScoreManager.starNumber; i++)
                stars[i].gameObject.SetActive(true);

            PlayerPrefs.SetInt("Bottle", oldTotalBottles + (ScoreManager.clockNumber + ScoreManager.starNumber));
            newTotalBottles = PlayerPrefs.GetInt("Bottle");
            
            SetUiTexts();
        }

        private void SetUiTexts()
        {
            newBottles.text = "+ " + (ScoreManager.clockNumber + ScoreManager.starNumber);
            
            newTotalBottlesLabel.text = newTotalBottles.ToString();

            levelNumber.text = "LEVEL " + Int32.Parse(SceneManager.GetActiveScene().name.Substring(5)) + " COMPLETO!";
            
            PlayerPrefs.Save();
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
