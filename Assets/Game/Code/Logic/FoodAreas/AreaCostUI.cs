using TMPro;
using UnityEngine;

namespace Game.Code.Logic.FoodAreas
{
    public class AreaCostUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _costText;
        
        public void Init(int cost)
        {
            SetCost(cost);
        }

        public void SetCost(int value)
        {
            _costText.SetText($"${value}");
        }
    }
}