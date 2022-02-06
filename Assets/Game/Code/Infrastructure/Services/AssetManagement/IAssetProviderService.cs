using UnityEngine;

namespace Game.Code.Infrastructure.Services.AssetManagement
{
    public interface IAssetProviderService : IService
    {
        public GameObject GetGameObject(string resourcesPath);
    }
}