using System;
using UnityEngine;
using Game.Code.Common;
using Game.Code.Players;

namespace Game.Code.Logic.Loots
{
    public class LootView : MonoBehaviour
    {
        public event Action<ICollector> Collected;
        
        [SerializeField] private LootType _lootType;

        public LootType LootType => _lootType;

        public void Show() => 
            gameObject.SetActive(true);

        public void Hide() =>
            gameObject.SetActive(false);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollector collector)) 
                Collected?.Invoke(collector);
        }
    }
}