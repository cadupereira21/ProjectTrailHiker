using System;
using System.Collections;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
        [SerializeField] private Button playButton;

        [SerializeField] private RectTransform levelDetailsField = null;
        [SerializeField] private RectTransform map;

        [Range(4.0f, 12.0f)]
        [SerializeField] private float animationSpeed = 7.5f;

        private int levelSelected = 0;

        public int LevelSelected => levelSelected;

        public void OnFaseClick(int faseNum)
        {
            StopAllCoroutines();

            //Debug.Log("Cliquei na fase " + faseNumber);

            if (!levelDetailsField.gameObject.activeInHierarchy)
            {
                levelDetailsField.gameObject.SetActive(true);   
            }

            // Funciona sem bug, porém sem animação
            //NewAnimate(); 
            
            // Bugado, porém animado
            StartCoroutine(Animate());

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
            //levelDetailsField.SetActive(false);
            //animation.Reset();
            SceneManager.LoadScene("Fase_" + levelSelected);
        }

        private IEnumerator Animate()
        {
            var t = 0.0f;
            while (t<1)
            {
                t = Time.deltaTime * animationSpeed;
                
                var mapPosition = map.anchoredPosition;
                var mapScale = map.localScale;
                var levelDetailsFieldPosition = levelDetailsField.anchoredPosition;
                
                var newMapPosition = new Vector2(332, -26);
                var newMapScale = new Vector3(0.35f, 0.35f, 0);
                var newLevelDetailsFieldPosition = new Vector2(-300.0f,0);

                map.anchoredPosition = Vector2.Lerp(mapPosition, newMapPosition, t);
                map.localScale = Vector3.Lerp(mapScale, newMapScale, t);
                levelDetailsField.anchoredPosition = Vector2.Lerp(levelDetailsFieldPosition, newLevelDetailsFieldPosition, t);
                yield return null;
            }

            yield break;
        }

        private void NewAnimate()
        {
            map.anchoredPosition = new Vector2(332, -26);
            map.localScale = new Vector3(0.35f, 0.35f, 0);
            levelDetailsField.anchoredPosition = new Vector2(-300.0f,0);
        }
    }
}
