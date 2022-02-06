using Game.Code.Common;
using Game.Code.StaticData;

namespace Game.Code.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadData();
        public FoodAreaStaticData GetAreaData(FoodAreaType type);
        public SitizenStaticData GetSitizenData(SitizenType type);
        public FoodStaticData GetFoodData(FoodType type);
        public LootStaticData GetLootData(LootType type);
    }
}