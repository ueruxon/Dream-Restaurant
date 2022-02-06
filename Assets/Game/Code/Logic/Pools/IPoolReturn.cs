using Game.Code.Common;

namespace Game.Code.Logic.Pools
{
    public interface IPoolReturn
    {
        void ReturnToPool(PooledObject pooledObject);
        void ReturnToPool(PooledObject pooledObject, SitizenType type);
    }
}