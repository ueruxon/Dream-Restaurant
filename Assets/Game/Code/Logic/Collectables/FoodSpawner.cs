using System.Collections;
using UnityEngine;
using Game.Code.Common;
using Game.Code.Infrastructure.Services.Factory;
using Game.Code.Players;
using Game.Code.StaticData;

namespace Game.Code.Logic.Collectables
{
    public class FoodSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FoodIndicator _foodIndicator;
        [SerializeField] private Transform _spawnPoint;
        [Space(2)] 
        [Header("Spawn Settings")]
        [SerializeField] private FoodType _foodType;
        [SerializeField] private float _cookFoodTimer = 1.5f;

        public FoodType FoodType => _foodType;

        private IGameFactoryService _gameFactory;
        private FoodStaticData _foodData;

        private ICollector _collector;

        private readonly WaitForSeconds _delay = new WaitForSeconds(0.1f);
        private bool isCook = false;

        public void Init(IGameFactoryService gameFactory, FoodStaticData foodStaticData)
        {
            _gameFactory = gameFactory;
            _foodData = foodStaticData;

            _foodIndicator.Init(_foodData.Icon);
            _foodIndicator.Hide();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isCook == false)
                if (other.TryGetComponent(out ICollector collector))
                {
                    _collector = collector;
                    isCook = true;
                    _foodIndicator.Show();

                    Cooking();
                }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isCook)
                if (other.TryGetComponent(out ICollector collector))
                {
                    isCook = false;

                    _foodIndicator.Hide();
                    _foodIndicator.SetIndicatorValue(0f);

                    StopAllCoroutines();
                }
        }

        private void MakeFood()
        {
            Food food = _gameFactory.CreateFood(_foodData.Template, _spawnPoint);
            food.Init(_foodType);
            
            _collector.Collect(food, _foodType);
        }

        private void Cooking()
        {
            StartCoroutine(CookRoutine());
        }

        private IEnumerator CookRoutine()
        {
            float currentTime = 0f;

            while (currentTime < _cookFoodTimer)
            {
                yield return _delay;

                currentTime += 0.1f;
                _foodIndicator.SetIndicatorValue(currentTime / _cookFoodTimer);
            }

            MakeFood();
            Cooking();
        }
    }
}