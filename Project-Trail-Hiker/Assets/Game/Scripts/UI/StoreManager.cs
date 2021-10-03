using System;
using Game.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class StoreManager : MonoBehaviour
    {
        [SerializeField] private Item[] itens;
        
        private PlayerInventory playerInventory;

        [SerializeField] private Image[] itemImageLabel;
        [SerializeField] private TextMeshProUGUI[] itemNameLabel;
        [SerializeField] private TextMeshProUGUI[] itemPriceLabel;
        [SerializeField] private TextMeshProUGUI[] itemTypeLabel;

        // Start is called before the first frame update
        void Start()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();

            SetLabels();
        }

        private void SetLabels()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                itemImageLabel[i].sprite = itens[i].Image;
                itemNameLabel[i].text = itens[i].Name;
                itemPriceLabel[i].text = itens[i].Price.ToString();
                itemTypeLabel[i].text = itens[i].Type.ToString();
            }
        }

        public void OnBuyClick(int itemNumber)
        {
            
        }
    }
}
