using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class EquipItem : MonoBehaviour
    {
        private PlayerInventory playerInventory;
        private InventoryManager inventoryManager;

        private List<Item> selectedItens;

        private string selectedSlot;
        
        [SerializeField] private RectTransform invPanelRect;
        [SerializeField] private RectTransform chooseItemRect;
        [Range(0.1f, 2.0f)]
        [SerializeField] private float animationDuration;

        [SerializeField] private GameObject itensLabel;
        [SerializeField] private Button[] itemButtons;
        [SerializeField] private Image[] buttonImagesLabel;
        [SerializeField] private TextMeshProUGUI[] buttonTextLabel;
        private void Start()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            inventoryManager = GetComponent<InventoryManager>();
            Debug.Log(invPanelRect.anchoredPosition + "\n" + chooseItemRect.anchoredPosition);
        }

        public void DisplayItens(string itemType)
        {
            Animate();
            
            if(!itensLabel.gameObject.activeInHierarchy)
                itensLabel.gameObject.SetActive(true);

            selectedItens = playerInventory.GetBoughtItensByType(itemType);
            selectedSlot = itemType;
            
            foreach (var but in itemButtons)
            {
                but.gameObject.SetActive(false);
            }

            if (selectedItens.Count == 0)
                return;
            

            var index = 0;
            foreach (Item i in selectedItens)
            {
                Debug.Log(i.Name);
                itemButtons[index].gameObject.SetActive(true);
                buttonImagesLabel[index].sprite = i.Image;
                buttonTextLabel[index].text = i.Name;
                ++index;
            }
        }

        public void OnItemClick(int buttonIndex)
        {
            Debug.Log(selectedSlot);
            Debug.Log(selectedItens[buttonIndex]);
            playerInventory.EquipItem(selectedItens[buttonIndex], selectedSlot);
            OnCancelClick();
        }

        public void OnCancelClick()
        {
            ReverseAnimation();
            itensLabel.gameObject.SetActive(false);
            inventoryManager.SetLabels();
        }

        private void Animate()
        {
            invPanelRect.DOAnchorPos(new Vector2(-200.0f, 3.4f), animationDuration);
            chooseItemRect.DOAnchorPos(new Vector2(577.0f, 65.0f), animationDuration);
            chooseItemRect.DOScale(new Vector3(0.5628148f, 0.7f, 1), animationDuration);
        }

        private void ReverseAnimation()
        {
            invPanelRect.DOAnchorPos(new Vector2(0.0f, 3.4f), animationDuration);
            chooseItemRect.DOAnchorPos(chooseItemRect.anchoredPosition - new Vector2(277.0f, 65.0f), animationDuration);
            chooseItemRect.DOScale(new Vector3(0.0f, 0.0f, 1), animationDuration);
        }
    }
}