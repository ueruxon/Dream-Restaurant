using Game.Code.Common;
using Game.Code.Logic.Collectables;
using Game.Code.Logic.FoodAreas;

namespace Game.Code.Logic.Sitizens
{
    public interface IClient
    {
        bool Serviced { get;}
        void SetSpot(Spot spot);
        void SetFood(Food food);

        FoodType GetFoodType();
    }
}