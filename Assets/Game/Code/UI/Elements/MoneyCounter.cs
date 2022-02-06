using System;
using Game.Code.Infrastructure.Services.Managers;
using TMPro;
using UnityEngine;

namespace Game.Code.UI.Elements
{
    public class MoneyCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyCountText;

        private IResourceManagerService _resourceManagerService;
        
        public void Init(IResourceManagerService resourceManagerService)
        {
            _resourceManagerService = resourceManagerService;
            _resourceManagerService.MoneyBalanceChanged += UpdateMoney;

            UpdateMoney(_resourceManagerService.GetMoneyBalance());
        }

        private void UpdateMoney(int moneyAmount)
        {
            _moneyCountText.SetText(moneyAmount.ToString());
        }

        private void OnDisable()
        {
            _resourceManagerService.MoneyBalanceChanged -= UpdateMoney;
        }
    }
}