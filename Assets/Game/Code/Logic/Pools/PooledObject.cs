using UnityEngine;
using Game.Code.Common;

namespace Game.Code.Logic.Pools
{
    public abstract class PooledObject : MonoBehaviour
    {
        private IPoolReturn _pool;

        public void AssignPool(IPoolReturn pool) => 
            _pool = pool;

        public void Enable() => 
            gameObject.SetActive(true);

        public void Disable() => 
            gameObject.SetActive(false);

        protected void ReturnToPool() => 
            _pool.ReturnToPool(this);
        
        protected void ReturnToPool(SitizenType type) => 
            _pool.ReturnToPool(this, type);
    }
}