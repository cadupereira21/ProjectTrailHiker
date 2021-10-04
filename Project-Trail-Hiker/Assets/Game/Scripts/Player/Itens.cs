using System;
using System.Collections;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class Itens : MonoBehaviour
    {
        private static Itens _instance;
        
        [SerializeField] private Item[] itens;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        public Item[] GetAllItens()
        {
            return itens;
        }

        public Item GetItemByName(string name)
        {
            foreach (Item i in itens)
            {
                if (i.Name.Equals(name))
                    return i;
            }
            
            Debug.Log("Item " + name + " does not exists!");
            return null;
        }
    }
}