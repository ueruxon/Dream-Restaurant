using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Code.Common;
using Game.Code.StaticData;

namespace Game.Code.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataAreasPath = "StaticData/Areas";
        private const string StaticDataSitizensPath = "StaticData/Sitizens";
        private const string StaticDataFoodsPath = "StaticData/Foods";
        private const string StaticDataLootsPath = "StaticData/Loots";

        private Dictionary<FoodAreaType, FoodAreaStaticData> _foodAreaDataByType;
        private Dictionary<SitizenType, SitizenStaticData> _sitizenDataByType;
        private Dictionary<FoodType, FoodStaticData> _foodDataByType;
        private Dictionary<LootType, LootStaticData> _lootDataByType;

        public void LoadData()
        {
            _foodAreaDataByType = Resources.LoadAll<FoodAreaStaticData>(StaticDataAreasPath)
                .ToDictionary(x => x.AreaType, x => x);;
            
            _sitizenDataByType = Resources.LoadAll<SitizenStaticData>(StaticDataSitizensPath)
                .ToDictionary(x => x.Type, x => x);;

            _foodDataByType = Resources.LoadAll<FoodStaticData>(StaticDataFoodsPath)
                .ToDictionary(x => x.Type, x => x);
            
            _lootDataByType = Resources.LoadAll<LootStaticData>(StaticDataLootsPath)
                .ToDictionary(x => x.LootType, x => x);
        }

        public FoodAreaStaticData GetAreaData(FoodAreaType type)
        {
            return _foodAreaDataByType.TryGetValue(type, out FoodAreaStaticData data)
                ? data
                : null;
        }

        public SitizenStaticData GetSitizenData(SitizenType type)
        {
            return _sitizenDataByType.TryGetValue(type, out SitizenStaticData data) 
                ? data 
                : null;
        }

        public FoodStaticData GetFoodData(FoodType type)
        {
            return _foodDataByType.TryGetValue(type, out FoodStaticData data) 
                ? data 
                : null;
        }

        public LootStaticData GetLootData(LootType type)
        {
            return _lootDataByType.TryGetValue(type, out LootStaticData data) 
                ? data 
                : null;
        }
    }
}