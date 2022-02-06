using UnityEngine;
using Game.Code.Common;
using Game.Code.Infrastructure.Services.Factory;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.StaticData;

namespace Game.Code.Logic.Loots
{
    public class LootSpawner
    {
        private float _maxDropRadius = 3f;
        private float _minDropRadius = 1.5f;
        
        private readonly IStaticDataService _staticDataService;
        private readonly IGameFactoryService _gameFactory;

        public LootSpawner(IStaticDataService staticDataService, IGameFactoryService gameFactory)
        {
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
        }

        public void SpawnLoot(LootType lootType, int lootCount, Vector3 startPosition)
        {
            for (int i = 0; i < lootCount; i++)
            {
                Vector3 endPosition = GetRandomEndPointOnRadius(startPosition);
                Vector3 centerPosition = (startPosition + endPosition) * .5f;
                Vector3 centerPositionWithOffset = (centerPosition + centerPosition) * .5f;
                
                endPosition += new Vector3(0, 0.1f);
                
                LootStaticData lootData = _staticDataService.GetLootData(lootType);
                Loot loot = _gameFactory.CreateLoot(startPosition);
                
                loot.Init(lootData, centerPositionWithOffset, endPosition);
                loot.StartMoving();
            }
        }
        
        private Vector3 GetRandomEndPointOnRadius(Vector3 startPosition)
        {
            float randomRadius = Random.Range(_minDropRadius, _maxDropRadius);
            
            float randomAngle = Random.Range(0f, 360f);
            float x = randomRadius * Mathf.Cos(randomAngle);
            float z = randomRadius * Mathf.Sin(randomAngle);

            return new Vector3(startPosition.x + x, .1f, startPosition.z + z);
        }
    }
}