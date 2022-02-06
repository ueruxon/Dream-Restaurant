using UnityEngine;
using Game.Code.Infrastructure.Services;
using Game.Code.Infrastructure.Services.Managers;
using Game.Code.Logic.Sitizens;

namespace Game.Code.Logic.FoodAreas
{
    public class FoodCourtTrigger : MonoBehaviour
    {
        private IFoodCourtManagerService _foodCourtManager;
        
        private void Awake() =>
            _foodCourtManager = ServiceLocator.Container.GetSingle<IFoodCourtManagerService>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IClient client))
            {
                _foodCourtManager.ClientVerification(client);
            }
        }
    }
}