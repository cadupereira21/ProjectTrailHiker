using System;
using Game.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private GameObject storePanel;
        [SerializeField] private GameObject inventoryPanel;

        [SerializeField] private Image[] itemImage = new Image[6];
        [SerializeField] private TextMeshProUGUI[] itemName = new TextMeshProUGUI[6];

        private PlayerInventory playerInventory;

        private EquipItem equipItem;

        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            equipItem = GetComponent<EquipItem>();
            
            SetLabels();
        }

        private void OnEnable()
        {
            SetLabels();
            Debug.Log("Fui ativado!");
        }

        public void SetLabels()
        {
            var equipedItens = playerInventory.GetAllEquipedItens();

            for (int i = 0; i < 6; i++)
            {
                if (equipedItens[i] != null)
                {
                    itemImage[i].gameObject.SetActive(true);
                    itemName[i].gameObject.SetActive(true);
                    
                    itemImage[i].sprite = equipedItens[i].Image;
                    itemName[i].text = equipedItens[i].Name;
                }
            }
        }

        public void OnStoreClick()
        {
            inventoryPanel.SetActive(false);
            storePanel.SetActive(true);
        }

        public void OnSlotClick(int slotNumber)
        {
            string itemType = "";
            
            switch (slotNumber)
            {
                case 1:
                    itemType = "Body";
                    break;
                case 2: 
                    itemType = "Legs";
                    break;
                case 3: 
                    itemType = "Foot";
                    break;
                case 4:
                    itemType = "Head";
                    break;
                case 5:
                    itemType = "RightHand";
                    break;
                case 6:
                    itemType = "LeftHand";
                    break;
            }
            
            equipItem.DisplayItens(itemType);
        }
    }
}