using Game.Code.Common;
using Game.Code.Logic.Collectables;
using UnityEngine;

namespace Game.Code.Players
{
    public interface ICollector
    {
        void Collect(Collectable collectable);
        void Collect(Collectable collectable, FoodType foodType);
        void AddMoney(int moneyAmount);
        Transform GetTransform();
        Vector3 GetCollectablePoint();
    }
}