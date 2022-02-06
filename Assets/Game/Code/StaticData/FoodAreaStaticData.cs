using Game.Code.Common;
using Game.Code.Logic.FoodAreas;
using UnityEngine;

namespace Game.Code.StaticData
{
    [CreateAssetMenu(fileName = "FoodArea Data", menuName = "Static Data/FoodArea Data", order = 0)]
    public class FoodAreaStaticData : ScriptableObject
    {
        public FoodAreaType AreaType;
        public int AreaCost = 50;
        public Table TableTemplate;
    }
}