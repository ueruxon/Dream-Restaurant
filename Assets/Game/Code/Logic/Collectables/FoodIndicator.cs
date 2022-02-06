using UnityEngine;
using UnityEngine.UI;

namespace Game.Code.Logic.Collectables
{
    public class FoodIndicator : MonoBehaviour
    {
        [SerializeField] private Image _food;
        [SerializeField] private Image _indicator;

        public void Init(Sprite foodSprite) => 
            _food.sprite = foodSprite;

        public void Show() => 
            gameObject.SetActive(true);

        public void Hide() => 
            gameObject.SetActive(false);

        public void SetIndicatorValue(float value) => 
            _indicator.fillAmount = value;
    }
}