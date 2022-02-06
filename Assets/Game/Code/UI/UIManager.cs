using Game.Code.Infrastructure.Services.Managers;
using Game.Code.UI.Elements;
using UnityEngine;

namespace Game.Code.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MoneyCounter _moneyCounter;

        public void Init(IResourceManagerService resourceManagerService)
        {
            _moneyCounter.Init(resourceManagerService);
        }
    }
}