using System;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class FasesManager : MonoBehaviour
    {
        [SerializeField] private Fase[] fase;

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI faseNumber;
        [SerializeField ]private Image faseImage;
        [SerializeField] private GameObject[] stars = new GameObject[5];
        [SerializeField] private TextMeshProUGUI[] timeScores = new TextMeshProUGUI[3];
        [SerializeField] private TextMeshProUGUI[] starNumber = new TextMeshProUGUI[3];

        public void OnFaseClick(int faseNumber)
        {
            //Play Animation
            //Draw UI
            title.text = fase[faseNumber - 1].FaseName.ToUpper();
            this.faseNumber.text = "LEVEL " + fase[faseNumber - 1].Number;
            faseImage.sprite = fase[faseNumber - 1].FaseImage;

            switch (fase[faseNumber - 1].Dificulty)
            {
                case 1:
                    stars[0].gameObject.SetActive(true);
                    break;
                case 2:
                    stars[0].gameObject.SetActive(true);
                    stars[1].gameObject.SetActive(true);
                    break;
                case 3:
                    stars[0].gameObject.SetActive(true);
                    stars[1].gameObject.SetActive(true);
                    stars[2].gameObject.SetActive(true);
                    break;
                case 4:
                    stars[0].gameObject.SetActive(true);
                    stars[1].gameObject.SetActive(true);
                    stars[2].gameObject.SetActive(true);
                    stars[3].gameObject.SetActive(true);
                    break;
                case 5:
                    stars[0].gameObject.SetActive(true);
                    stars[1].gameObject.SetActive(true);
                    stars[2].gameObject.SetActive(true);
                    stars[3].gameObject.SetActive(true);
                    stars[4].gameObject.SetActive(true);
                    break;
            }

            timeScores[0].text = "- " + fase[faseNumber-1].MaxTimeScore[0] + ":" + fase[faseNumber-1].MaxTimeScore[1].ToString("00");
            timeScores[1].text = "- " + fase[faseNumber-1].AverageTimeScore[0] + ":" + fase[faseNumber-1].AverageTimeScore[1].ToString("00");
            timeScores[2].text = "- " + fase[faseNumber-1].MinTimeScore[0] + ":" + fase[faseNumber-1].MinTimeScore[1].ToString("00");

            starNumber[0].text = "- " + fase[faseNumber-1].MaxStarScore + " Acidentes";
            starNumber[1].text = "- " + fase[faseNumber-1].AverageStarScore + " Acidentes";
            starNumber[2].text = "- " + fase[faseNumber-1].MinStarScore + " Acidentes";
        }

        public void OnPlayClick()
        {
            var faseNumber = Int32.Parse(this.faseNumber.text.Substring(5));
            SceneManager.LoadScene("Fase_" + faseNumber);
        }
    }
}
