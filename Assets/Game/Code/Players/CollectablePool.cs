using System.Collections.Generic;
using Game.Code.Common;
using Game.Code.Logic.Collectables;
using UnityEngine;

namespace Game.Code.Players
{
    public class CollectablePool<T> where T : Collectable
    {
        private Transform _container;

        private Stack<T> _attachedCollectionStack;
        private Dictionary<FoodType, Stack<T>> _foodAttachedCollectionByType;

        private Vector3 _collectablePosition;

        public CollectablePool(Transform container, int poolCapacity = 10)
        {
            _container = container;
            
            _attachedCollectionStack = new Stack<T>();
            _foodAttachedCollectionByType = new Dictionary<FoodType, Stack<T>>()
            {
                [FoodType.Donut] = new Stack<T>(),
                [FoodType.Sandwich] = new Stack<T>(),
            };

            _collectablePosition = Vector3.zero;
        }

        // for test
        public bool IsEmpty()
        {
            foreach (KeyValuePair<FoodType,Stack<T>> item in _foodAttachedCollectionByType)
            {
                if (item.Value.Count > 0)
                    return false;
            }

            return true;
        }

        public void AddItem(Collectable collectable)
        {
            AttachToCollection(collectable);
            
            _attachedCollectionStack.Push((T) collectable);
        }

        public void AddItem(Collectable collectable, FoodType foodType)
        {
            AttachToCollection(collectable);
            
            _foodAttachedCollectionByType[foodType].Push((T) collectable);
        }

        // проверка по типу. Принимать будет тип

        public bool HasCollectedItem() =>
            _attachedCollectionStack.Count > 0;

        public bool HasCollectedItem(FoodType foodType) =>
            _foodAttachedCollectionByType[foodType].Count > 0;

        public T GetCollectableItem(Transform container)
        {
            Collectable collectable = _attachedCollectionStack.Pop();
            DetachFromCollection(collectable, container);
            return (T) collectable;
        }

        public T GetCollectableItem(Transform container, FoodType foodType)
        {
            Collectable collectable = _foodAttachedCollectionByType[foodType].Pop();
            
            DetachFromCollection(collectable, container);
            return (T) collectable;
        }

        private void AttachToCollection(Collectable collectable)
        {
            collectable
                .GetTransform()
                .SetParent(_container);

            CalculateNextPosition(collectable.GetHeight());
            collectable.MoveTo(_collectablePosition);
        }

        private void DetachFromCollection(Collectable collectable, Transform container)
        {
            collectable
                .GetTransform()
                .SetParent(container, true);

            collectable.MoveTo(Vector3.zero);
            
            RecalculateCollectablePositions(collectable.GetHeight());
        }

        private void CalculateNextPosition(float collectableHeight)
        {
            _collectablePosition += new Vector3(0, collectableHeight, 0);
        }

        private void RecalculateCollectablePositions(float collectableHeight)
        {
            Vector3 newCollectablePosition = Vector3.zero;
            
            foreach (Transform child in _container)
            {
                child.transform.localPosition = newCollectablePosition;
                newCollectablePosition += new Vector3(0, collectableHeight, 0);
            }
            
            newCollectablePosition -= new Vector3(0, collectableHeight, 0);
            
            _collectablePosition = newCollectablePosition;
        }
    }
}