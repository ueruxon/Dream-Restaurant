using System.Collections.Generic;
using Game.Code.Common;

namespace Game.Code.Logic.Pools
{
    class Pool<T> : IPoolReturn where T : PooledObject
    {
        private readonly Dictionary<SitizenType, Queue<T>> _queueSitizenByType;
        private readonly Queue<T> _inactiveObjects = new Queue<T>();

        public Pool()
        {
            _queueSitizenByType = new Dictionary<SitizenType, Queue<T>>()
            {
                [SitizenType.Normal] = new Queue<T>(),
                [SitizenType.Vip] = new Queue<T>()
            };
        }

        public void Add(T obj) =>
            obj.AssignPool(this);

        public void ReturnToPool(PooledObject obj)
        {
            obj.Disable();
            _inactiveObjects.Enqueue((T) obj);
        }

        public void ReturnToPool(PooledObject obj, SitizenType type)
        {
            obj.Disable();
            _queueSitizenByType[type].Enqueue((T) obj);
        }

        public bool HasInactiveObjects() =>
            _inactiveObjects.Count > 0;

        public bool HasInactiveObjects(SitizenType type) =>
            _queueSitizenByType[type].Count > 0;

        public T GetInactiveObject()
        {
            var inactiveObject = _inactiveObjects.Dequeue();
            inactiveObject.Enable();

            return inactiveObject;
        }

        public T GetInactiveObject(SitizenType type)
        {
            var inactiveObject = _queueSitizenByType[type].Dequeue();
            inactiveObject.Enable();

            return inactiveObject;
        }
    }
}