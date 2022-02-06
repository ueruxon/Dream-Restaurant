using Game.Code.Common;
using UnityEngine;

namespace Game.Code.StaticData
{
    [CreateAssetMenu(fileName = "Loot Data", menuName = "Static Data/Loot Data", order = 4)]
    public class LootStaticData : ScriptableObject
    {
        [SerializeField] private int _amount;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Color _color;
        [SerializeField] private LootType _lootType;

        public int Amount => _amount;
        public Sprite Icon => _icon;
        public LootType LootType => _lootType;
        public Color Color => _color;
    }
}