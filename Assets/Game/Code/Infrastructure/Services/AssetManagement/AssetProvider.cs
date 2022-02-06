using UnityEngine;

namespace Game.Code.Infrastructure.Services.AssetManagement
{
    public class AssetProvider : IAssetProviderService
    {
        public GameObject GetGameObject(string resourcesPath)
        {
            var template = Resources.Load<GameObject>(resourcesPath);
            return template;
        }
    }
}