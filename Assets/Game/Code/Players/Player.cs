using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Code.Common;
using Game.Code.Infrastructure.Services.Inputs;
using Game.Code.Infrastructure.Services.Managers;
using Game.Code.Logic.Collectables;
using Game.Code.Logic.FoodAreas;
using Game.Code.Logic.Sitizens;

namespace Game.Code.Players
{
    public class Player : MonoBehaviour, ICollector, IResourceOwner
    {
        [Header("References")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private Transform _collectableContainer;

        private IInputService _inputService;
        private IResourceManagerService _resourceManagerService;
        private Camera _camera;
        
        private PlayerMovement _playerMovement;
        private CollectablePool<Food> _foodPool;

        public void Init(IInputService inputService, IResourceManagerService resourceManagerService)
        {
            _inputService = inputService;
            _resourceManagerService = resourceManagerService;
            
            _camera = Camera.main;

            _playerMovement = new PlayerMovement(_inputService, _characterController, _playerAnimator, _camera);
            _foodPool = new CollectablePool<Food>(_collectableContainer);
        }

        private void Update()
        {
            _playerMovement.Move(transform, Time.deltaTime);
        }

        public void AddMoney(int moneyAmount) => 
            _resourceManagerService.AddMoney(moneyAmount);

        public void Collect(Collectable collectable) { }
        
        public void Collect(Collectable collectable, FoodType foodType)
        {
            _foodPool.AddItem(collectable, foodType);
            
            CheckFoodOnHand();
        }

        public Transform GetTransform() => 
            transform;
        
        public Vector3 GetCollectablePoint()
        {
             Vector3 currentPosition = transform.position;
             float offsetY = _characterController.height * .5f;
             
             return new Vector3(currentPosition.x, currentPosition.y + offsetY, currentPosition.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Table table))
            {
                if (table.TryGetPendingClients(out Dictionary<IClient, Spot> pendingClients))
                {
                    TryToServeClients(pendingClients);
                }
            }
        }

        private void TryToServeClients(Dictionary<IClient,Spot> pendingClients)
        {
            foreach (KeyValuePair<IClient,Spot> pendingClient in pendingClients)
            {
                IClient client = pendingClient.Key;
                // требует определенный тип еды
                FoodType foodType = client.GetFoodType();
                
                if (_foodPool.HasCollectedItem(foodType))
                {
                    if (client.Serviced == false)
                    {
                        Spot spot = pendingClient.Value;
                        Food food = _foodPool.GetCollectableItem(spot.GetFoodPoint(), foodType);
                    
                        client.SetFood(food);
                    }
                }

                CheckFoodOnHand();
            }
        }

        private void CheckFoodOnHand()
        {
            int resultWeight = _foodPool.IsEmpty() ? 0 : 1;

            _playerAnimator.SelectLayerWeight(resultWeight);
        }
    }
}