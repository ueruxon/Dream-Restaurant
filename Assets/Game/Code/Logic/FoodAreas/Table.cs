using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Game.Code.Logic.Sitizens;

namespace Game.Code.Logic.FoodAreas
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private List<Spot> _spots;
        
        private Dictionary<IClient, Spot> _pendingClients;

        public void Init()
        {
            _pendingClients = new Dictionary<IClient, Spot>();

            transform.DOShakeScale(1f, 0.4f);

            foreach (Spot spot in _spots)
            {
                spot.ClientPending += OnClientPending;
                spot.ClientServiced += OnClientServiced;
            }
        }

        public int GetSpotCount() => 
            _spots.Count;

        public List<Spot> GetSpotList() => _spots;

        public bool TryGetPendingClients(out Dictionary<IClient, Spot> clients)
        {
            if (_pendingClients.Count > 0)
            {
                clients = _pendingClients;
                return true;
            }
            
            clients = _pendingClients;
            return false;
        }

        private void OnClientPending(IClient client, Spot spot) => 
            _pendingClients.Add(client, spot);

        private void OnClientServiced(IClient client, Spot spot) => 
            _pendingClients.Remove(client);
    }
}
