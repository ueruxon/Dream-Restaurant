using System;
using UnityEngine;

namespace Game.Code.Infrastructure.Services.Managers
{
    public class ResourceManager : IResourceManagerService
    {
        public event Action<int> MoneyBalanceChanged;
        
        private int _moneyBalance;

        public ResourceManager(int defaultMoneyAmount)
        {
            _moneyBalance = defaultMoneyAmount;
        }

        public int GetMoneyBalance() => _moneyBalance;
        
        public bool TrySpendMoney(int amount)
        {
            if (_moneyBalance > 0)
            {
                _moneyBalance -= amount;
                MoneyBalanceChanged?.Invoke(_moneyBalance);

                return true;
            }

            _moneyBalance = 0;
            return false;
        }

        public void AddMoney(int amount)
        {
            int currentMoney = Mathf.Clamp(_moneyBalance + amount, 0, 9999);
            _moneyBalance = currentMoney;

            MoneyBalanceChanged?.Invoke(_moneyBalance);
        }
        
    }
}