using UnityEngine;

namespace Game.Scripts.Player
{
    [CreateAssetMenu(fileName = "Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private string name = "";
        [SerializeField] private ItemType type;
        [Range(1, 200)]
        [SerializeField] private int price = 0;
        [SerializeField] private Sprite image = null;

        public string Name => name;
        public ItemType Type => type;
        public int Price => price;
        public Sprite Image => image;
    }
}