using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [CreateAssetMenu(fileName = "Fase")]
    public class Fase : ScriptableObject
    { 
        [Header("Informações gerais")]
        [SerializeField] private string faseName = "";
        [SerializeField] private int number = 0;
        [Range(1, 5)]
        [Tooltip("Um número de 1 a 5 representando o nível de dificuldade da fase (representado por estrelas)")]
        [SerializeField] private int dificulty = 0;

        [SerializeField] private Sprite faseImage = null;
        [SerializeField] private bool isLocked;
        
        [Header("Pontuação de Tempo")]
        [Tooltip("Elemento 1 = Minutos | Elemento 2 = Segundos")]
        [SerializeField] private int[] maxTimeScore = new int[2];
        [Tooltip("Elemento 1 = Minutos | Elemento 2 = Segundos")]
        [SerializeField] private int[] averageTimeScore = new int[2];
        [Tooltip("Elemento 1 = Minutos | Elemento 2 = Segundos")]
        [SerializeField] private int[] minTimeScore = new int[2];
        
        [Header("Pontuação de Estrelas")]
        [SerializeField] private int maxStarScore = 0;
        [SerializeField] private int averageStarScore = 0;
        [SerializeField] private int minStarScore = 0;
        
        [Header("Itens Necessários")]
        [SerializeField] private string[] necessaryItens;

        public string FaseName => faseName;
        public int Number => number;
        public int Dificulty => dificulty;
        public Sprite FaseImage => faseImage;
        public int[] MaxTimeScore => maxTimeScore;
        public int[] AverageTimeScore => averageTimeScore;
        public int[] MinTimeScore => minTimeScore;
        public int MaxStarScore => maxStarScore;
        public int AverageStarScore => averageStarScore;
        public int MinStarScore => minStarScore;
        public string[] NecessaryItens => necessaryItens;
        public bool IsLocked => isLocked;

        public void Unlock()
        {
            isLocked = false;
        }
    }
}