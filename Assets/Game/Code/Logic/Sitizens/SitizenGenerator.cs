using UnityEngine;
using Game.Code.Common;
using Game.Code.Infrastructure.Services.Factory;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.Logic.Pools;
using Game.Code.StaticData;

namespace Game.Code.Logic.Sitizens
{
    public class SitizenGenerator
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IGameFactoryService _gameFactory;

        private Pool<Sitizen> _pool;

        private int _sitizenCount = 0;

        public SitizenGenerator(IStaticDataService staticDataService, IGameFactoryService gameFactory)
        {
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
            
            _pool = new Pool<Sitizen>();
        }

        public void CreateSitizens(SitizenSpawnDefaultConfig[] sitizenSpawnDefaultConfigs)
        {
            foreach (SitizenSpawnDefaultConfig config in sitizenSpawnDefaultConfigs)
            {
                for (int i = 0; i < config.DefaultQuantity; i++) 
                    Generate(config.Type);
            }
        }

        public Sitizen GetSitizen(SitizenType type)
        {
            return _pool.HasInactiveObjects(type) 
                ? _pool.GetInactiveObject(type) 
                : Generate(type);
        }

        public SitizenStaticData GetSitizenData(SitizenType type) => 
            _staticDataService.GetSitizenData(type);

        private Sitizen Generate(SitizenType type)
        {
            SitizenStaticData data = _staticDataService.GetSitizenData(type);
            Vector3 defaultSpawnPoint = Vector3.zero;
            
            Sitizen sitizen = _gameFactory.CreateSitizen(data.Template, data.GetRandomVisual(), defaultSpawnPoint);

            NameEdit(sitizen, type);
            
            _pool.Add(sitizen);
            _pool.ReturnToPool(sitizen, type);

            return sitizen;
        }

        private void NameEdit(Sitizen sitizen, SitizenType type)
        {
            _sitizenCount++;
            
            switch (type)
            {
                case SitizenType.Normal:
                    sitizen.name = "NORMAL " + sitizen.name + " " + _sitizenCount;
                    break;
                case SitizenType.Vip:
                    sitizen.name = "VIP " + sitizen.name + " " + _sitizenCount;
                    break;
            }
        }
    }
}