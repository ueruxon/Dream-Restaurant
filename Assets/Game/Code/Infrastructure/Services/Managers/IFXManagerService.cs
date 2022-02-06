using UnityEngine;

namespace Game.Code.Infrastructure.Services.Managers
{
    public interface IFXManagerService : IService
    {
        void PlayFX(GameObject fxTemplate, Vector3 at);
        void PlayFX(GameObject fxTemplate, Transform container);
    }
}