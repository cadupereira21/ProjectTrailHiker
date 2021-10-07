using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private static PlayerInventory _instance;
        private Itens itens;
        
        private int bottles;
        private string equipedItem_Head;
        private string equipedItem_Body;
        private string equipedItem_LeftHand;
        private string equipedItem_RightHand;
        private string equipedItem_Legs;
        private string equipedItem_Foot;
        private ArrayList boughtItens = new ArrayList();
        private int numberOfBoughtItens;

        public int Bottles => bottles;

        private void Awake()
        {
            // GainBottles(1000);
            // PlayerPrefs.DeleteKey("boughtItem_0");
            // PlayerPrefs.DeleteKey("boughtItem_1");
            // PlayerPrefs.DeleteKey("boughtItem_2");
            // PlayerPrefs.DeleteKey("boughtItem_3");
            // PlayerPrefs.DeleteKey("equipedItem_Body");
            // PlayerPrefs.DeleteKey("equipedItem_Legs");
            // PlayerPrefs.DeleteKey("equipedItem_Foot");
            // PlayerPrefs.DeleteKey("equipedItem_Head");
            // PlayerPrefs.DeleteKey("equipedItem_RightHand");
            // PlayerPrefs.DeleteKey("equipedItem_LeftHand");

            if (_instance == null)
                _instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            itens = FindObjectOfType<Itens>();

            DontDestroyOnLoad(gameObject);

            AttAllAtributes();
            
            Debug.Log("Equiped Itens: \n" + "Body: " + equipedItem_Body + "\n Legs: " + equipedItem_Legs + "\n Foot: " + equipedItem_Foot + "\n Head: " + equipedItem_Head
            + "\n LeftHand: " + equipedItem_LeftHand + "\n RightHand: " + equipedItem_RightHand);
            
            Debug.Log("Number of itens bought: " + boughtItens.Count + "\n Number of bottles owned: " + bottles);
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

        private void CheckBoughtItens()
        {
            for (int i = 0; i < numberOfBoughtItens; i++)
            {
                var itenName = PlayerPrefs.GetString("boughtItem_" + i);
                var allItens = itens.GetAllItens();

                foreach (var VARIABLE in allItens)
                {
                    if (VARIABLE.Name.Equals(itenName))
                        boughtItens.Add(VARIABLE);
                }
            }
        }

        public void GainBottles(int quantity)
        {
            PlayerPrefs.SetInt("bottles", quantity+bottles);
            bottles = PlayerPrefs.GetInt("bottles");

            PlayerPrefs.Save();
            Debug.Log("You gained " + quantity + " bottles!");
        }

        private void SpendBottles(int quantity)
        {
            PlayerPrefs.SetInt("bottles", bottles-quantity);
            bottles = PlayerPrefs.GetInt("bottles");

            PlayerPrefs.Save();
            Debug.Log("You've spent " + quantity + " bottles");
        }

        public int EquipItem(Item item, string slotName)
        {
            string itemType = "";
            
            if (slotName.Equals("LeftHand") || slotName.Equals("RightHand"))
                itemType = "Hand";
            else
                itemType = slotName;

            if (itemType.Equals("Head") || itemType.Equals("Body") || itemType.Equals("Hand") || itemType.Equals("Legs") || itemType.Equals("Foot"))
            {
                if (!item.Type.ToString().Equals(itemType))
                {
                    Debug.Log("EquipItem(Item item, string slotName) -> slotName does not match with item type!");
                    return -1;  
                }

                if (itemType == "Hand")
                {
                    switch (slotName)
                    {
                        case "LeftHand":
                            if (equipedItem_RightHand == item.Name)
                            {
                                Debug.Log("Este item já está equipado na outra mão!");
                                return -1;
                            }
                            break;
                        case "RightHand":
                            if (equipedItem_LeftHand == item.Name)
                            {
                                Debug.Log("Este item já está equipado na outra mão!");
                                return -1;
                            }
                            break;
                    }
                }
                
                PlayerPrefs.SetString("equipedItem_" + slotName, item.name);
                
                equipedItem_Head = PlayerPrefs.GetString("equipedItem_Head");
                equipedItem_Body = PlayerPrefs.GetString("equipedItem_HeadequipedItem_Body");
                equipedItem_LeftHand = PlayerPrefs.GetString("equipedItem_LeftHand");
                equipedItem_RightHand = PlayerPrefs.GetString("equipedItem_RightHand");
                equipedItem_Legs = PlayerPrefs.GetString("equipedItem_Legs");
                equipedItem_Foot = PlayerPrefs.GetString("equipedItem_Foot");
                PlayerPrefs.Save();
                Debug.Log("You've equiped the item " + item.Name + " at the slot " + slotName);
                return 1;
            }

            Debug.LogError("EquipItem(Item item, string slotName) ->  slotName " + slotName + " is not a valid field!");
            return -1;
        }

        public void BuyItem(Item item)
        {
            SpendBottles(item.Price);
            
            PlayerPrefs.SetString("boughtItem_" + boughtItens.Count, item.Name);
            boughtItens.Add(item);
            PlayerPrefs.SetInt("numberOfBoughtItens", PlayerPrefs.GetInt("numberOfBoughtItens") + 1);
            PlayerPrefs.Save();
            Debug.Log("You've bought the " + item.Name + " you now have " + bottles + " bottles!");
        }

        public ArrayList GetAllBoughtItens()
        {
            return boughtItens;
        }

        public List<Item> GetBoughtItensByType(string itemType)
        {
            List<Item> aux = new List<Item>();

            if (itemType.Equals("LeftHand") || itemType.Equals("RightHand"))
                itemType = "Hand";
            
            foreach (Item VARIABLE in boughtItens)
            {
                if (VARIABLE.Type.ToString().Equals(itemType))
                {
                    aux.Add(VARIABLE);
                }
            }

            return aux;
        }

        public Item SearchBoughtItem(string itemName)
        {
            foreach (Item i in boughtItens)
            {
                if (i.Name.Equals(itemName))
                {
                    //Debug.Log("Found item " + itemName);
                    return i;   
                }
            }

            Debug.LogWarning("Could not find item with name \"" + itemName + "\" at " + boughtItens.GetType() + " boughItens!");
            return null;
        }

        public Item[] GetAllEquipedItens()
        {

            var equipedItens = new Item[6];
            var boughtItens = GetAllBoughtItens();

            if (equipedItem_Body != "")
            {
                equipedItens[0] = SearchBoughtItem(equipedItem_Body);
            }
            
            if (equipedItem_Legs != "")
            {
                equipedItens[1] = SearchBoughtItem(equipedItem_Legs);
            }
            
            if (equipedItem_Foot != "")
            {
                equipedItens[2] = SearchBoughtItem(equipedItem_Foot);
            }
            
            if (equipedItem_Head != "")
            {
                equipedItens[3] = SearchBoughtItem(equipedItem_Head);
            }
            
            if (equipedItem_RightHand != "")
            {
                equipedItens[4] = SearchBoughtItem(equipedItem_RightHand);
            }
            
            if (equipedItem_LeftHand != "")
            {
                equipedItens[5] = SearchBoughtItem(equipedItem_LeftHand);
            }
            
            return equipedItens;
        }

        private void AttAllAtributes()
        {
            CheckKeys();
            bottles = PlayerPrefs.GetInt("bottles");
            equipedItem_Head = PlayerPrefs.GetString("equipedItem_Head");
            equipedItem_Body = PlayerPrefs.GetString("equipedItem_Body");
            equipedItem_LeftHand = PlayerPrefs.GetString("equipedItem_LeftHand");
            equipedItem_RightHand = PlayerPrefs.GetString("equipedItem_RightHand");
            equipedItem_Legs = PlayerPrefs.GetString("equipedItem_Legs");
            equipedItem_Foot = PlayerPrefs.GetString("equipedItem_Foot");
            numberOfBoughtItens = PlayerPrefs.GetInt("numberOfBoughtItens");
            boughtItens = new ArrayList();
            CheckBoughtItens();
        }
    }
}