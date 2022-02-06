using System.Collections.Generic;
using Game.Code.Logic.FoodAreas;

namespace Game.Code.Infrastructure.Services.Managers
{
    public interface ITableHolder
    {
        void AddTable(Table table);
        void RemoveTable(Table table);
        List<Table> GetAllTables();
    }
}