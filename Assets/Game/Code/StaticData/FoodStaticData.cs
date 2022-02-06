using UnityEngine;
using Game.Code.Common;
using Game.Code.Logic.Collectables;

namespace Game.Code.StaticData
{
    [CreateAssetMenu(fileName = "Food Data", menuName = "Static Data/Food Data", order = 2)]
    public class FoodStaticData : ScriptableObject
    {
        public FoodType Type;
        public Sprite Icon;
        public Food Template;
    }
}