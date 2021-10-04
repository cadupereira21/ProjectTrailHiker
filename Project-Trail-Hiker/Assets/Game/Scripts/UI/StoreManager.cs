using System;
using Game.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class StoreManager : MonoBehaviour
    {
        [SerializeField] private GameObject storePanel;
        [SerializeField] private GameObject inventoryPanel;
        
        [SerializeField] private Item[] itens;
        
        private PlayerInventory playerInventory;

        [SerializeField] private Button[] itemButtons;
        [SerializeField] private Image[] itemImageLabel;
        [SerializeField] private TextMeshProUGUI[] itemNameLabel;
        [SerializeField] private TextMeshProUGUI[] itemPriceLabel;
        [SerializeField] private TextMeshProUGUI[] itemTypeLabel;
        [SerializeField] private TextMeshProUGUI currentWage;

        // Start is called before the first frame update
        void Start()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
        }

        private void Update()
        {
            SetLabels();
        }

        private void SetLabels()
        {
            currentWage.text = playerInventory.Bottles.ToString();
            
            var boughItens = playerInventory.GetAllBoughtItens();
            
            for (int i = 0; i < itens.Length; i++)
            {
                if (playerInventory.SearchBoughtItem(itens[i].Name) == null)
                    itemPriceLabel[i].text = itens[i].Price.ToString();
                else
                {
                    itemPriceLabel[i].text = "Já obtido";
                    itemButtons[i].interactable = false;
                }
                itemImageLabel[i].sprite = itens[i].Image;
                itemNameLabel[i].text = itens[i].Name;
                itemTypeLabel[i].text = itens[i].Type.ToString();
            }
        }

        public void OnBuyClick(int itemNumber)
        {
            var itemIndex = itemNumber - 1;

            if (playerInventory.Bottles < itens[itemIndex].Price)
            {
                // APARECER MENSAGEM
                Debug.Log("You don't have enough money for this!");
            }
            else
            {
                playerInventory.BuyItem(itens[itemIndex]);   
            }
        }

        public void OnInventoryClick()
        {
            storePanel.SetActive(false);
            inventoryPanel.SetActive(true);
        }
    }
}
