namespace Game.Code.Infrastructure.Services
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        public static ServiceLocator Container => _instance ?? (_instance = new ServiceLocator());

        public void Register<TService>(TService implementation) where TService : IService {
            Implementation<TService>.ServiceInstance = implementation;
        }

        public TService GetSingle<TService>() where TService : IService {
            return Implementation<TService>.ServiceInstance;
        }

        private class Implementation<TService> where TService : IService {
            public static TService ServiceInstance;
        }
    }
}