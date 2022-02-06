using System;

namespace Game.Code.Infrastructure.Services.Managers
{
    public interface IResourceManagerService : IService
    {
        event Action<int> MoneyBalanceChanged;

        int GetMoneyBalance();
        bool TrySpendMoney(int amount);
        void AddMoney(int amount);
    }
}