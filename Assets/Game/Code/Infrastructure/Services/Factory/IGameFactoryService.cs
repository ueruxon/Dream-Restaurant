using Game.Code.Logic.Collectables;
using UnityEngine;
using Game.Code.Logic.FoodAreas;
using Game.Code.Logic.Loots;
using Game.Code.Logic.Sitizens;
using Game.Code.Players;

namespace Game.Code.Infrastructure.Services.Factory
{
    public interface IGameFactoryService : IService
    {
        Player CreatePlayer(Transform at);
        GameObject CreateHud();
        Food CreateFood(Food foodTemplate, Transform spawnPoint);
        Table CreateTable(Table tableTemplate, Transform container);
        GameObject CreateFx(GameObject template, Transform container);
        GameObject CreateFx(GameObject template, Vector3 at);
        Sitizen CreateSitizen(Sitizen dataTemplate, GameObject getRandomVisual, Vector3 defaultSpawnPoint);
        Loot CreateLoot(Vector3 spawnPosition);
    }
}