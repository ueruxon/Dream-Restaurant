using System.Collections.Generic;
using Game.Code.Logic.FoodAreas;
using Game.Code.Logic.Sitizens;

namespace Game.Code.Infrastructure.Services.Managers
{
    public class FoodCourtManager : IFoodCourtManagerService
    {
        private readonly List<Table> _tables;
        private readonly List<IClient> _clients;

        public FoodCourtManager()
        {
            _tables = new List<Table>();
            _clients = new List<IClient>();
        }

        public void ClientVerification(IClient client)
        {
            if (_clients.Contains(client) == false)
            {
                _clients.Add(client);
                TryPutClientOnSpot(client);
            }
            else
                _clients.Remove(client);
        }

        private bool TryPutClientOnSpot(IClient client)
        {
            foreach (Table table in _tables)
            {
                foreach (Spot spot in table.GetSpotList())
                {
                    if (spot.IsOccupied == false)
                    {
                        client.SetSpot(spot);
                        spot.SetClient(client);
                        
                        return true;
                    }
                }
            }

            return false;
        }

        public void AddTable(Table table) => 
            _tables.Add(table);

        public void RemoveTable(Table table) => 
            _tables.Remove(table);

        public List<Table> GetAllTables() => _tables;
    }
}