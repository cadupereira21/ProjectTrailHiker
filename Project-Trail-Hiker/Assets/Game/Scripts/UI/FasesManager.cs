using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

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
        [SerializeField] private Button playButton;

        [SerializeField] private RectTransform levelDetailsField = null;
        [SerializeField] private RectTransform map;
        // private Vector2 levelDetailsFieldPosition;
        // private Vector2 mapPosition;
        // private Vector2 mapScale;

        [Range(0.0f, 5.0f)]
        [SerializeField] private float animationDuration = 1.0f;

        private int levelSelected = 0;

        public int LevelSelected => levelSelected;

        private void Start()
        {
            DOTween.Init(true, true, LogBehaviour.Verbose);
            // levelDetailsFieldPosition = levelDetailsField.anchoredPosition;
            // mapPosition = map.anchoredPosition;
            // mapScale = map.localScale;
        }

        public void OnFaseClick(int faseNum)
        {
            if (!levelDetailsField.gameObject.activeInHierarchy)
            {
                levelDetailsField.gameObject.SetActive(true);   
            }

            Animate();

            levelSelected = faseNum;

            playButton.interactable = !fase[levelSelected - 1].IsLocked;

            title.text = fase[levelSelected - 1].FaseName.ToUpper();
            this.faseNumber.text = "LEVEL " + fase[levelSelected - 1].Number;
            faseImage.sprite = fase[levelSelected - 1].FaseImage;
            
            foreach (var t in stars)
                t.SetActive(false);

            for (int i = 0; i < stars.Length; i++)
                if (i < fase[levelSelected - 1].Dificulty)
                    stars[i].SetActive(true);

            timeScores[0].text = "- " + fase[levelSelected-1].MaxTimeScore[0] + ":" + fase[levelSelected-1].MaxTimeScore[1].ToString("00");
            timeScores[1].text = "- " + fase[levelSelected-1].AverageTimeScore[0] + ":" + fase[levelSelected-1].AverageTimeScore[1].ToString("00");
            timeScores[2].text = "- " + fase[levelSelected-1].MinTimeScore[0] + ":" + fase[levelSelected-1].MinTimeScore[1].ToString("00");

            starNumber[0].text = "- " + fase[levelSelected-1].MaxStarScore + " Acidentes";
            starNumber[1].text = "- " + fase[levelSelected-1].AverageStarScore + " Acidentes";
            starNumber[2].text = "- " + fase[levelSelected-1].MinStarScore + " Acidentes";
        }

        public void OnPlayClick()
        {
            SceneManager.LoadScene("Fase_" + levelSelected);
        }

        private void Animate()
        {
            Debug.Log(levelDetailsField.anchoredPosition + "\n" + map.anchoredPosition);
            map.DOAnchorPos(new Vector2(332, -26), animationDuration);
            map.DOScale(new Vector3(0.35f, 0.35f, 0), animationDuration);
            levelDetailsField.DOAnchorPos(new Vector2(-300.0f, 0), animationDuration);
            Debug.Log("Terminei de Animar");
        }

        private void NewAnimate()
        {
            map.anchoredPosition = new Vector2(332, -26);
            map.localScale = new Vector3(0.35f, 0.35f, 0);
            levelDetailsField.anchoredPosition = new Vector2(-300.0f,0);
        }
    }
}
