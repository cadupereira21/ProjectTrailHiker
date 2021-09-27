using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private int bottles;
        private string equipedItem_Head;
        private string equipedItem_Body;
        private string equipedItem_LeftHand;
        private string equipedItem_RightHand;
        private string equipedItem_Legs;
        private string equipedItem_Foot;
        private ArrayList boughtItens = new ArrayList();

        public int Bottles => bottles;

        private void Awake()
        {
            CheckKeys();
            bottles = PlayerPrefs.GetInt("bottles");
            equipedItem_Head = PlayerPrefs.GetString("equipedItem_Head");
            equipedItem_Body = PlayerPrefs.GetString("equipedItem_Body");
            equipedItem_LeftHand = PlayerPrefs.GetString("equipedItem_LeftHand");
            equipedItem_RightHand = PlayerPrefs.GetString("equipedItem_RightHand");
            equipedItem_Legs = PlayerPrefs.GetString("equipedItem_Legs");
            equipedItem_Foot = PlayerPrefs.GetString("equipedItem_Foot");
            boughtItens = new ArrayList();

            StartCoroutine(CheckBoughtItens());
        }

        private void CheckKeys()
        {
            if (!PlayerPrefs.HasKey("bottles"))
            {
                PlayerPrefs.SetInt("bottles", 0);
            }
            
            if (!PlayerPrefs.HasKey("equipedItem_Head"))
            {
                PlayerPrefs.SetString("equipedItem_Head", "");
            }
            
            if (!PlayerPrefs.HasKey("equipedItem_Body"))
            {
                PlayerPrefs.SetString("equipedItem_Body", "");
            }
            
            if (!PlayerPrefs.HasKey("equipedItem_LeftHand"))
            {
                PlayerPrefs.SetString("equipedItem_LeftHand", "");
            }
            
            if (!PlayerPrefs.HasKey("equipedItem_RightHand"))
            {
                PlayerPrefs.SetString("equipedItem_RightHand", "");
            }
            
            if (!PlayerPrefs.HasKey("equipedItem_Legs"))
            {
                PlayerPrefs.SetString("equipedItem_Legs", "");
            }
            
            if (!PlayerPrefs.HasKey("equipedItem_Foot"))
            {
                PlayerPrefs.SetString("equipedItem_Foot", "");
            }
        }

        public void GainBottles(int quantity)
        {
            PlayerPrefs.SetInt("bottles", quantity+bottles);
            bottles = PlayerPrefs.GetInt("bottles");

            PlayerPrefs.Save();
        }

        public void SpendBottles(int quantity)
        {
            PlayerPrefs.SetInt("bottles", bottles-quantity);
            bottles = PlayerPrefs.GetInt("bottles");

            PlayerPrefs.Save();
        }

        public int EquipItem(Item item, string slotName)
        {
            if (slotName.Equals("Head") || slotName.Equals("Body") || slotName.Equals("LeftHand") ||
                slotName.Equals("RightHand") || slotName.Equals("Legs") || slotName.Equals("Foot"))
            {
                if(slotName.Equals("LeftHand") || slotName.Equals("RightHand"))
                    if (!item.Type.Equals("Hand"))
                    {
                        Debug.Log("EquipItem(Item item, string slotName) -> slotName does not match with item type!");
                        return -1;   
                    }

                if (!item.Type.Equals(slotName))
                {
                    Debug.Log("EquipItem(Item item, string slotName) -> slotName does not match with item type!");
                    return -1;  
                }
                    

                PlayerPrefs.SetString("equipedItem_" + slotName, item.name);
                
                equipedItem_Head = PlayerPrefs.GetString("equipedItem_Head");
                equipedItem_Body = PlayerPrefs.GetString("equipedItem_HeadequipedItem_Body");
                equipedItem_LeftHand = PlayerPrefs.GetString("equipedItem_LeftHand");
                equipedItem_RightHand = PlayerPrefs.GetString("equipedItem_RightHand");
                equipedItem_Legs = PlayerPrefs.GetString("equipedItem_Legs");
                equipedItem_Foot = PlayerPrefs.GetString("equipedItem_Foot");
                PlayerPrefs.Save();
                return 1;
            }

            Debug.LogError("EquipItem(Item item, string slotName) ->  slotName " + slotName + " is not a valid field!");
            return -1;
        }

        public void HasBoughtItem(Item item)
        {
            boughtItens.Add(item);
            PlayerPrefs.SetString("boughtItem_" + boughtItens.IndexOf(item), item.Name);
            PlayerPrefs.Save();
        }

        private IEnumerator CheckBoughtItens()
        {
            int index = 0;
            var itens = FindObjectsOfType<Item>();
            Item item;

            while (true)
            {
                if (PlayerPrefs.HasKey("boughtItem_" + index))
                {
                    item = null;
                    
                    foreach (var i in itens)
                    {
                        if (i.Name.Equals(PlayerPrefs.GetString("boughtItem_" + index)))
                            item = i;
                    }
                    
                    boughtItens.Add(item);
                    index += 1;
                    yield return null;
                }
                else
                {
                    Debug.Log("Number of itens bought: " + index);
                    yield break;
                }
            }
        }

        public ArrayList GetAllBoughtItens()
        {
            var boughtItensClone = (ArrayList) boughtItens.Clone();
            return boughtItensClone;
        }
    }
}