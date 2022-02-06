using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Code.Common;
using Game.Code.Infrastructure.Services.Managers;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.Logic.FoodAreas;
using Game.Code.Logic.Loots;
using Game.Code.StaticData;

namespace Game.Code.Logic.Sitizens
{
    public class SitizenSpawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> _sitizenWaypointList;
        [SerializeField] private int _chanceSpawnVip = 15;

        private ITableHolder _foodCourtManager;
        private IStaticDataService _staticDataService;
        private IFXManagerService _fxManager;
        
        private SitizenGenerator _sitizenGenerator;
        private LootSpawner _lootSpawner;

        private WaitForSeconds _cooldownBetweenSitizenWave = new WaitForSeconds(6f);
        private WaitForSeconds _spawnInterval = new WaitForSeconds(3f);

        private List<FoodType> _foodTypes;

        private int _additionalSitizenCount = 2;

        public void Init(ITableHolder foodCourtManager, IStaticDataService staticDataService,
            IFXManagerService fxManagerService, SitizenGenerator generator, LootSpawner lootSpawner)
        {
            _foodCourtManager = foodCourtManager;
            _fxManager = fxManagerService;
            _staticDataService = staticDataService;
            
            _sitizenGenerator = generator;
            _lootSpawner = lootSpawner;

            _foodTypes = new List<FoodType>()
            {
                FoodType.Donut,
                FoodType.Sandwich
            };
        }

        public void StartSpawnSitizen() =>
            StartCoroutine(SpawnRoutine());

        public void StopSpawn() =>
            StopAllCoroutines();

        private IEnumerator SpawnRoutine()
        {
            // поменять условие
            while (true)
            {
                List<Table> tables = _foodCourtManager.GetAllTables();

                foreach (Table table in tables.ToArray())
                {
                    for (int i = 0; i < table.GetSpotCount() + _additionalSitizenCount; i++)
                    {
                        SpawnRandomSitizen();
                        yield return new WaitForSeconds(Random.Range(3f, 6f));
                    }
                }

                //yield return _cooldownBetweenSitizenWave;
            }
        }

        private void SpawnRandomSitizen()
        {
            int currentValue = Random.Range(0, 100);

            SitizenType currentType = SitizenType.Normal;

            if (_chanceSpawnVip > currentValue)
                currentType = SitizenType.Vip;

            Sitizen sitizen = _sitizenGenerator.GetSitizen(currentType);
            SitizenStaticData sitizenData = _sitizenGenerator.GetSitizenData(currentType);
            SitizenWaypointNetwork waypointNetwork = new SitizenWaypointNetwork(_sitizenWaypointList);

            sitizen.Init(sitizenData, _fxManager, waypointNetwork, GetRandomFoodData(), _lootSpawner, transform.position);
        }

        private FoodStaticData GetRandomFoodData()
        {
            int index = Random.Range(0, _foodTypes.Count);
            return _staticDataService.GetFoodData(_foodTypes[index]);
        }
    }
}