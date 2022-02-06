using Game.Code.Infrastructure.Services.AssetManagement;
using Game.Code.Logic.Collectables;
using UnityEngine;
using Game.Code.Logic.FoodAreas;
using Game.Code.Logic.Loots;
using Game.Code.Logic.Sitizens;
using Game.Code.Players;
using Unity.Mathematics;

namespace Game.Code.Infrastructure.Services.Factory
{
    public class GameFactory : IGameFactoryService
    {
        private readonly IAssetProviderService _assetProvider;

        public GameFactory(IAssetProviderService assetProvider) => 
            _assetProvider = assetProvider;

        public Player CreatePlayer(Transform at)
        {
            var template = _assetProvider.GetGameObject(AssetPath.PlayerPath);
            return Object.Instantiate(template, at.position, Quaternion.identity)
                .GetComponent<Player>();
        }

        public GameObject CreateHud()
        {
            var template = _assetProvider.GetGameObject(AssetPath.HudPath);
            return Object.Instantiate(template);
        }

        public Food CreateFood(Food foodTemplate, Transform spawnPoint) => 
            Object.Instantiate(foodTemplate, spawnPoint.position, Quaternion.identity);

        public Table CreateTable(Table tableTemplate, Transform container) => 
            Object.Instantiate(tableTemplate, container);

        public GameObject CreateFx(GameObject template, Transform container)
        {
            Vector3 point = container.position + Vector3.up;
            return Object.Instantiate(template, point, Quaternion.identity, container);
        }

        public GameObject CreateFx(GameObject template, Vector3 at) => 
            Object.Instantiate(template, at, quaternion.identity);

        public Sitizen CreateSitizen(Sitizen template, GameObject visualTemplate, Vector3 defaultSpawnPoint)
        {
            Sitizen sitizen = Object.Instantiate(template, defaultSpawnPoint, Quaternion.identity);
            GameObject visual = Object.Instantiate(visualTemplate, sitizen.transform);
            visual.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                
            SitizenAnimator animator = visual.AddComponent<SitizenAnimator>();
            
            sitizen.Construct(animator);
            
            return sitizen;
        }

        public Loot CreateLoot(Vector3 spawnPosition)
        {
            var template = _assetProvider.GetGameObject(AssetPath.LootPath);
            return Object.Instantiate(template, spawnPosition, Quaternion.identity)
                .GetComponent<Loot>();
        }
    }
}