using Game.Code.Common;
using UnityEngine;
using Game.Code.Infrastructure.Services;
using Game.Code.Infrastructure.Services.AssetManagement;
using Game.Code.Infrastructure.Services.Factory;
using Game.Code.Infrastructure.Services.Inputs;
using Game.Code.Infrastructure.Services.Managers;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.Logic.Camera;
using Game.Code.Logic.Collectables;
using Game.Code.Logic.FoodAreas;
using Game.Code.Logic.Loots;
using Game.Code.Logic.Sitizens;
using Game.Code.Players;
using Game.Code.StaticData;
using Game.Code.UI;

namespace Game.Code.Core
{
    public class GameInstaller
    {
        private const string InitialPoint = "InitialPoint";

        private IAssetProviderService _assetProvider;
        private IStaticDataService _staticDataService;
        private IGameFactoryService _gameFactory;
        private IInputService _inputService;
        private IFoodCourtManagerService _foodCourtManager;
        private IResourceManagerService _resourceManager;
        private IFXManagerService _fxManager;

        public GameInstaller()
        {
            if (Application.isEditor)
            {
                Application.targetFrameRate = 70;
            }
            
            RegisterServices();
        }
        
        public void InitLevel()
        {
            InitPlayer();
            InitHud();
            InitFoodArea();
            InitFoodSpawners();
            InitSitizenSpawner();
        }
        

        private void RegisterServices()
        {
            RegisterStaticData();

            _assetProvider = new AssetProvider();
            _gameFactory = new GameFactory(_assetProvider);
            _inputService = new InputService();
            _foodCourtManager = new FoodCourtManager();
            _resourceManager = new ResourceManager(100);
            _fxManager = new FXManager(_gameFactory);
            
            ServiceLocator.Container.Register(_assetProvider);
            ServiceLocator.Container.Register(_gameFactory);
            ServiceLocator.Container.Register(_inputService);
            ServiceLocator.Container.Register(_foodCourtManager);
            ServiceLocator.Container.Register(_resourceManager);
        }

        private void RegisterStaticData()
        {
            _staticDataService = new StaticDataService();
            _staticDataService.LoadData();
            ServiceLocator.Container.Register(_staticDataService);
        }

        private void InitPlayer()
        {
            var initialPoint = GameObject.FindWithTag(InitialPoint);
            Player player = _gameFactory.CreatePlayer(initialPoint.transform);

            player.Init(_inputService, _resourceManager);

            CameraFollow(player.gameObject);
        }
        
        private void InitHud()
        {
            UIManager uiManager = _gameFactory.CreateHud()
                .GetComponent<UIManager>();
            
            uiManager.Init(_resourceManager);
        }

        private void InitFoodSpawners()
        {
            FoodSpawner[] spawners = Object.FindObjectsOfType<FoodSpawner>();

            foreach (FoodSpawner spawner in spawners)
            {
                FoodStaticData data = _staticDataService.GetFoodData(spawner.FoodType);
                spawner.Init(_gameFactory, data);
            }
        }

        private void InitFoodArea()
        {
            FoodArea[] foodAreas = Object.FindObjectsOfType<FoodArea>();
            
            foreach (FoodArea area in foodAreas)
            {
                FoodAreaStaticData data = _staticDataService.GetAreaData(area.GetAreaType());
                area.Init(_gameFactory, _fxManager, _resourceManager, _foodCourtManager, data);
            }
        }

        private void InitSitizenSpawner()
        {
            SitizenSpawner spawner = Object.FindObjectOfType<SitizenSpawner>();

            SitizenSpawnDefaultConfig[] defaultConfigs = 
            {
                new SitizenSpawnDefaultConfig(SitizenType.Normal, 15),
                new SitizenSpawnDefaultConfig(SitizenType.Vip, 7)
            };
                
            // init sitizen by default
            SitizenGenerator sitizenGenerator = new SitizenGenerator(_staticDataService, _gameFactory);
            sitizenGenerator.CreateSitizens(defaultConfigs);
            
            LootSpawner lootSpawner = new LootSpawner(_staticDataService, _gameFactory);

            spawner.Init(_foodCourtManager,  _staticDataService, _fxManager, sitizenGenerator, lootSpawner);
            // тест
            spawner.StartSpawnSitizen();
        }
        
        private void CameraFollow(GameObject target) =>
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(target);
    }
}