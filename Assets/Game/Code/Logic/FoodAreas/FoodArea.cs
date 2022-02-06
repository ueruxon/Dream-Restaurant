using System.Collections;
using UnityEngine;
using Game.Code.Common;
using Game.Code.Infrastructure.Services.Factory;
using Game.Code.Infrastructure.Services.Managers;
using Game.Code.Players;
using Game.Code.StaticData;

namespace Game.Code.Logic.FoodAreas
{
    public class FoodArea : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SellZone _sellZone;
        [SerializeField] private AreaCostUI _areaUI;
        [SerializeField] private GameObject _constructionFx;
        
        [Space(2)]
        [SerializeField] private FoodAreaType _areaType;
        [SerializeField] private bool boughtByDefault;

        private IGameFactoryService _gameFactory;
        private IResourceManagerService _resourceManager;
        private ITableHolder _foodCourtManager;
        private IFXManagerService _fxManager;
        private FoodAreaStaticData _data;
        
        private bool _isBought;
        private int _spendingAmount = 1;
        private int _currentAreaMoney;

        public void Init(IGameFactoryService gameFactory, IFXManagerService fxManager, IResourceManagerService resourceManager, 
            ITableHolder foodCourtManager, FoodAreaStaticData staticData)
        {
            _gameFactory = gameFactory;
            _foodCourtManager = foodCourtManager;
            _resourceManager = resourceManager;
            _fxManager = fxManager;
            
            _data = staticData;
            
            _isBought = boughtByDefault;
            _currentAreaMoney = _data.AreaCost;
            _areaUI.Init(_data.AreaCost);

            if (_isBought)
                SpawnTable();
        }

        private void OnEnable()
        {
            _sellZone.StartSpending += OnStartSpending;
            _sellZone.StopSpending += OnStopSpending;
        }
        
        private void OnDisable()
        {
            _sellZone.StartSpending -= OnStartSpending;
            _sellZone.StopSpending -= OnStopSpending;
        }

        private void SpawnTable()
        {
            _sellZone.Hide();

            Table table = _gameFactory.CreateTable(_data.TableTemplate, transform);
            table.Init();
            
            _foodCourtManager.AddTable(table);
            
            _fxManager.PlayFX(_constructionFx, transform);
        }

        private void OnStartSpending(IResourceOwner owner)
        {
            StartCoroutine(SpendingRoutine());
        }

        private void OnStopSpending(IResourceOwner owner)
        {
            StopAllCoroutines();
        }

        public FoodAreaType GetAreaType() => _areaType;

        private IEnumerator SpendingRoutine()
        {
            while (_isBought == false)
            {
                if (_resourceManager.TrySpendMoney(_spendingAmount))
                {
                    _currentAreaMoney -= _spendingAmount;

                    if (_currentAreaMoney <= 0)
                    {
                        _isBought = true;
                        _currentAreaMoney = 0;
                        
                        SpawnTable();
                    }
                    
                    _areaUI.SetCost(_currentAreaMoney);
                }
                else
                    break;

                yield return null;
            }
        }
    }
}