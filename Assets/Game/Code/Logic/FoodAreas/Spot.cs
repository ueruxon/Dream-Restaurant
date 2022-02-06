using System;
using Game.Code.Logic.Sitizens;
using UnityEngine;

namespace Game.Code.Logic.FoodAreas
{
    public class Spot : MonoBehaviour
    {
        public event Action<IClient, Spot> ClientPending;
        public event Action<IClient, Spot> ClientServiced;

        [SerializeField] private Transform _foodPoint;
        [SerializeField] private Transform _chairPoint;

        private bool _isOccupied;
        public bool IsOccupied => _isOccupied;

        private IClient _currentClient;

        public void SetClient(IClient client)
        {
            _isOccupied = true;
            _currentClient = client;
        }

        public void Pending() => 
            ClientPending?.Invoke(_currentClient, this);

        public void Clear() {
            ClientServiced?.Invoke(_currentClient, this);
            
            _isOccupied = false;
            _currentClient = null;
        }

        public Transform GetFoodPoint() => _foodPoint;

        public Transform GetSpotPoint() => _chairPoint;
    }
}