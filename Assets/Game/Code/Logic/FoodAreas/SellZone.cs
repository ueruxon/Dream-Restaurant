using System;
using Game.Code.Players;
using UnityEngine;

namespace Game.Code.Logic.FoodAreas
{
    public class SellZone : MonoBehaviour
    {
        public event Action<IResourceOwner> StartSpending;
        public event Action<IResourceOwner> StopSpending;

        private IResourceOwner _resourceOwner;

        private float spendTimerMax = 1.5f;
        private float spendCurrentTime = 0f;

        private bool stay = false;
        private bool spend = false;

        public void Show() =>
            gameObject.SetActive(true);

        public void Hide() =>
            gameObject.SetActive(false);

        private void Update()
        {
            if (stay)
                CheckSpend();
        }

        private void CheckSpend()
        {
            if (spend == false)
            {
                spendCurrentTime += Time.deltaTime;

                if (spendCurrentTime >= spendTimerMax)
                {
                    StartSpending?.Invoke(_resourceOwner);
                    spend = true;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (spend == false)
                if (other.TryGetComponent(out IResourceOwner resourceOwner))
                {
                    stay = true;
                    _resourceOwner = resourceOwner;
                }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IResourceOwner resourceOwner))
            {
                stay = false;
                spend = false;
                spendCurrentTime = 0f;

                StopSpending?.Invoke(resourceOwner);
            }
        }
    }
}