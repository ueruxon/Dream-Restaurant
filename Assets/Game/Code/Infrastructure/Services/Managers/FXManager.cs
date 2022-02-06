using Game.Code.Infrastructure.Services.Factory;
using UnityEngine;

namespace Game.Code.Infrastructure.Services.Managers
{
    public class FXManager : IFXManagerService
    {
        private readonly IGameFactoryService _gameFactory;

        public FXManager(IGameFactoryService gameFactory) => 
            _gameFactory = gameFactory;

        public void PlayFX(GameObject fxTemplate, Vector3 at) => 
            _gameFactory.CreateFx(fxTemplate, at);

        public void PlayFX(GameObject fxTemplate, Transform container) => 
            _gameFactory.CreateFx(fxTemplate, container);
    }
}