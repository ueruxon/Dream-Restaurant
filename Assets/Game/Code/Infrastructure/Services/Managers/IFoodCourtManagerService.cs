using Game.Code.Logic.Sitizens;

namespace Game.Code.Infrastructure.Services.Managers
{
    public interface IFoodCourtManagerService : IService, ITableHolder
    {
        void ClientVerification(IClient client);
    }
}